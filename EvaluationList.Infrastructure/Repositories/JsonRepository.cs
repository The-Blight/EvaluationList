using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using EvaluationList.Domain.Interfaces;

namespace EvaluationList.Infrastructure.Repositories;

/// <summary>
/// Репозиторий для сохранения данных в JSON-файлы с использованием паттерна DTO.
/// Автоматически маппит сущности перед сохранением и после чтения.
/// </summary>
/// <typeparam name="TEntity">Тип доменной сущности (реализует IEntity).</typeparam>
/// <typeparam name="TDto">Тип DTO, используемый для сериализации.</typeparam>
public class JsonRepository<TEntity, TDto> : IRepository<TEntity> 
    where TEntity : IEntity
    where TDto : IDto
{
    
    /// <summary>
    /// Путь к файлу
    /// </summary>
    /// <exception cref="ArgumentException"> бросает исключение в случае отсуствие пути</exception>
    public string FilePath
    {
        get;
        init
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Путь к файлу не должен быть пустым", nameof(FilePath));

            field = value; 
        }
    }
    private readonly IMapper<TEntity, TDto> _mapper;

    
    /// <summary>
    /// Настройки сериализатора. 
    /// Отключено экранирование кириллицы для читаемости JSON-файлов.
    /// </summary>
    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true
    };


    /// <summary>
    /// Инициализирует репозиторий с указанием файла и маппера.
    /// </summary>
    /// <param name="filePath"> путь к файлу.</param>
    /// <param name="mapper">Реализация маппера для трансляции Entity в DTO и обратно.</param>
    public JsonRepository(string filePath, IMapper<TEntity, TDto> mapper)
    {
        
        FilePath = filePath; 
        _mapper = mapper;
    }


    
    /// <inheritdoc/>
    public void Create(TEntity entity)
    {
        var dtos = ReadDtoFromJson();
        dtos.Add(_mapper.ToDto(entity));
        WriteDtoToJson(dtos);
    }  

    /// <inheritdoc/>
    public TEntity? GetById(Guid id)
    {
        var dto = ReadDtoFromJson().FirstOrDefault(d =>
            _mapper.ToDomain(d).Id == id);

        return dto is null ? default : _mapper.ToDomain(dto);
    }

    /// <inheritdoc/>
    public IEnumerable<TEntity> GetAll()
    {
        var dtos = ReadDtoFromJson();
        return dtos.Select(d => _mapper.ToDomain(d));
    }

    /// <inheritdoc/>
    public void Update(TEntity entity)
    {
        var dtos = ReadDtoFromJson();
        var index = dtos.FindIndex(d => _mapper.ToDomain(d).Id == entity.Id);

        if (index == -1) return;

        dtos[index] = _mapper.ToDto(entity);
        WriteDtoToJson(dtos);
    }

    
    /// <inheritdoc/>
    public void Delete(Guid id)
    {
        var dtos = ReadDtoFromJson();
        var itemToRemove = dtos.FirstOrDefault(d => _mapper.ToDomain(d).Id == id);

        if (itemToRemove is null) return;

        dtos.Remove(itemToRemove);
        WriteDtoToJson(dtos);
    }


    /// <summary>
    /// Читает и десериализует список DTO из файла
    /// </summary>
    /// <returns>Список объектов DTO или пустой список, если файл отсутствует.</returns>
    private List<TDto> ReadDtoFromJson()
    {
        if (!File.Exists(FilePath)) return [];

        var json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<TDto>>(json, _options) ?? [];
    }

    /// <summary>
    /// Сериализует список DTO и записывает в файл.
    /// </summary>      
    /// <param name="list">Список сущностей для записи.</param>
    private void WriteDtoToJson(List<TDto> list)
    {
        var json = JsonSerializer.Serialize(list, _options);
        File.WriteAllText(FilePath, json);
    }
}
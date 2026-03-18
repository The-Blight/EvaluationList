using System.Text.Json;
using EvaluationList.Domain.Interfaces;

namespace EvaluationList.Infrastructure.Repositories;

/// <summary>
/// Универсальный репозиторий для работы с данными в формате JSON.
/// Обеспечивает персистентность объектов, реализующих интерфейс <see cref="IEntity"/>.
/// </summary>
/// <typeparam name="T">Тип сущности, ограниченный интерфейсом <see cref="IEntity"/>.</typeparam>
public class JsonRepository<T> : IRepository<T> where T : IEntity
{
    private readonly string _filePath;

    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        IncludeFields = true
    };


    public JsonRepository(string fileName)
    {
        _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
    }


    /// <summary>
    /// Добавляет новую сущность в JSON-хранилище.
    /// </summary>
    /// <param name="entity">Объект для сохранения.</param>
    public void Create(T entity)
    {
        var all = GetAll().ToList();
        all.Add(entity);
        WriteInJson(all);
    }

    /// <summary>
    /// Выполняет поиск сущности по её уникальному идентификатору.
    /// </summary>
    /// <param name="id">GUID искомой сущности.</param>
    /// <returns>Возвращает сущность или <see langword="null"/>, если объект не найден.</returns>
    public T? GetById(Guid id)
    {
        return ReadFromJson().FirstOrDefault(i => i.Id == id);
    }

    /// <summary>
    /// Извлекает все записи из JSON-файла.
    /// </summary>
    /// <returns>Коллекция всех объектов типа <typeparamref name="T"/>.</returns>
    public IEnumerable<T> GetAll() => ReadFromJson();


    /// <summary>
    /// Обновляет существующую сущность в файле на основе совпадения <see cref="IEntity.Id"/>.
    /// </summary>
    /// <param name="entity">Обновленный объект.</param>
    /// <remarks>Если объект с таким Id не найден, операция будет проигнорирована.</remarks>
    public void Update(T entity)
    {
        var all = ReadFromJson();
        var index = all.FindIndex(i => i.Id == entity.Id);

        if (index == -1) return;
        all[index] = entity;
        WriteInJson(all);
    }

    /// <summary>
    /// Удаляет сущность из хранилища по идентификатору.
    /// </summary>
    /// <param name="id">GUID объекта, который нужно удалить.</param>
    public void Delete(Guid id)
    {
        var all = ReadFromJson();
        var itemToRemove = all.FirstOrDefault(i => i.Id == id);

        if (itemToRemove is null) return;
        all.Remove(itemToRemove);
        WriteInJson(all);
    }

    /// <summary>
    /// Приватный метод для считывания данных с диска и их десериализации.
    /// </summary>
    /// <returns>Список объектов или пустой список, если файл отсутствует.</returns>
    private List<T> ReadFromJson()
    {
        if (!File.Exists(_filePath)) return [];

        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<T>>(json, _options) ?? [];
    }

    /// <summary>
    /// Приватный метод для сериализации списка объектов и записи в файл.
    /// </summary>
    /// <param name="list">Список сущностей для сохранения.</param>
    private void WriteInJson(List<T> list)
    {
        var json = JsonSerializer.Serialize(list, _options);
        File.WriteAllText(_filePath, json);
    }
}
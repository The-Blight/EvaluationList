using System;
using System.Text.RegularExpressions;
using EvaluationList.Domain.Interfaces;

namespace EvaluationList.Domain.Entities.Staff;

/// <summary>
/// Доменная сущность: Эксперт (Член жюри).
/// Отвечает за хранение и строгую валидацию ФИО.
/// </summary>
public partial class Expert : IEntity
{
    /// <inheritdoc/>
    public Guid Id { get; init; } = Guid.CreateVersion7();

    /// <summary>
    /// Регулярное выражение для проверки ФИО.
    /// Требует заглавную первую букву и допускает дефис (для двойных имен/фамилий).
    /// </summary>
    [GeneratedRegex(@"^[A-ZА-ЯЁ][a-zа-яё]+(-[A-ZА-ЯЁa-zа-яё]+)*$")]
    private static partial Regex NameRegex();

    
    
    /// <summary>
    /// Имя эксперта. Обязательное поле.
    /// </summary>
    /// <exception cref="ArgumentException">Выбрасывается, если формат имени некорректен.</exception>
    public required string FirstName
    {
        get => field;
        init
        {
            var trimmed = value?.Trim() ?? string.Empty;
            if (!NameRegex().IsMatch(trimmed))
                throw new ArgumentException($"Некорректный формат имени: {value}");

            field = trimmed;
        }
    }

    
    
    /// <summary>
    /// Отчество эксперта. Допускает null или пустую строку (преобразуется в null).
    /// </summary>
    /// <exception cref="ArgumentException">Выбрасывается, если отчество указано, но не соответствует формату.</exception>
    public string? Patronymic
    {
        get => field;
        init
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                field = null!;
                return;
            }

            var trimmed = value.Trim(); 
            if (!NameRegex().IsMatch(trimmed))
                throw new ArgumentException($"Некорректный формат отчества: {value}");

            field = trimmed;
        }
    }

    
    /// <summary>
    /// Фамилия эксперта. Обязательное поле.
    /// </summary>
    /// <exception cref="ArgumentException">Выбрасывается, если формат фамилии некорректен.</exception>
    public required string LastName
    {
        get => field;
        init
        {
            var trimmed = value?.Trim() ?? string.Empty;
            if (!NameRegex().IsMatch(trimmed))
                throw new ArgumentException($"Некорректный формат фамилии: {value}");

            field = trimmed;
        }
    }

    /// <summary>
    /// Возвращает полное ФИО эксперта, собранное через пробел.
    /// Лишние пробелы удаляются, если отчество отсутствует.
    /// </summary>
    public string FullName => $"{LastName} {FirstName} {Patronymic}".Trim();
}
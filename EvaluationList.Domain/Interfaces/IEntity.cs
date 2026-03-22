using System;

namespace EvaluationList.Domain.Interfaces;
/// <summary>
/// Базовый контракт для всех доменных сущностей системы.
/// Гарантирует наличие уникального идентификатора для работы с репозиториями.
/// </summary>
public interface IEntity
{ 
    /// <summary>
    /// Уникальный идентификатор сущности. 
    /// Рекомендуется использовать Guid.CreateVersion7() для упорядочивания по времени.
    /// </summary>
    Guid Id { get; init; }
}
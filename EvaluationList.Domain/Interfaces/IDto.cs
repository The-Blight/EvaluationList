using System;

namespace EvaluationList.Domain.Interfaces;


/// <summary>
/// Базовый интерфейс для объектов передачи данных (DTO).
/// Позволяет инфраструктуре (репозиториям) быстро находить объекты по ID без десериализации в Домен.
/// </summary>
public interface IDto
{
    /// <summary>Уникальный идентификатор DTO.</summary>
    public Guid Id { get; init; }
    
}
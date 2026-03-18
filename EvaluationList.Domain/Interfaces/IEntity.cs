using System;

namespace EvaluationList.Domain.Interfaces;
/// <summary>
/// Базовый интерфейс для всех сущностей 
/// </summary>
public interface IEntity
{ 
    Guid Id { get; init; }
}
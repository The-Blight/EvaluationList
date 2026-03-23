using System;
using System.Collections.Generic;

namespace EvaluationList.Domain.Interfaces;

/// <summary>
/// Базовый интерфейс репозитория.
/// Абстрагирует домен от конкретной реализации хранилища (БД, файлы, память).
/// </summary>
/// <typeparam name="TEntity">Тип сущности, с которой работает репозиторий.</typeparam>
public interface IRepository<TEntity> where TEntity : IEntity
{
    /// <summary>Сохраняет новую сущность в хранилище.</summary>
    /// <param name="entity">Доменная сущность для сохранения.</param>
    void Create(TEntity entity);

    /// <summary>Ищет сущность по ее уникальному идентификатору.</summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <returns>Найденная сущность или null, если объект не найден.</returns>
    TEntity? GetById(Guid id);

    /// <summary>Возвращает все сущности данного типа из хранилища.</summary>
    /// <returns>Коллекция всех найденных сущностей.</returns>
    IEnumerable<TEntity> GetAll();

    /// <summary>Обновляет существующую сущность в хранилище.</summary>
    /// <param name="entity">Сущность с обновленными данными.</param>
    void Update(TEntity entity);

    /// <summary>Удаляет сущность из хранилища по ее идентификатору.</summary>
    /// <param name="id">Идентификатор удаляемой сущности.</param>
    void Delete(Guid id);
}
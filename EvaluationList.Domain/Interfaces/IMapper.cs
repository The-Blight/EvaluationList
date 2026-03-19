namespace EvaluationList.Domain.Interfaces;



/// <summary>
/// Базовый контракт для трансляции доменных сущностей в DTO и обратно.
/// Обеспечивает независимость домена от механизмов хранения.
/// </summary>
/// <typeparam name="TEntity">Тип доменной сущности.</typeparam>
/// <typeparam name="TDto">Тип объекта передачи данных.</typeparam>
public interface IMapper<TEntity, TDto>
{
    /// <summary>Преобразует доменную сущность в плоский DTO для сохранения.</summary>
    TDto ToDto(TEntity entity);
    /// <summary>Восстанавливает полноценную доменную сущность из плоского DTO.</summary>
    TEntity ToDomain(TDto dto); 
}


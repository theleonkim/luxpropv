# Repository Pattern Implementation

This directory contains the implementation of the Repository Pattern using the `RepositoryBase<T>` class for all models in the Luxprop.Data.Models namespace.

## Repository Structure

Each repository follows this pattern:
- **Interface**: `I{ModelName}Repository` - Defines the contract
- **Implementation**: `{ModelName}Repository` - Inherits from `RepositoryBase<T>`

## Available Repositories

### Core Models
1. **AgenteRepository** - Manages agent entities
   - Get by UsuarioId, CodigoAgente, Sucursal
   
2. **UsuarioRepository** - Manages user entities
   - Get by Email, Email/Password, Active users, By Role
   - Email existence validation

3. **PropiedadRepository** - Manages property entities
   - Get by AgenteId, UbicacionId, Estado, Price range
   - Active properties filtering

4. **ClienteRepository** - Manages client entities
   - Get by Cedula, UsuarioId, TipoCliente
   - Cedula existence validation

### Chat System
5. **ChatMessageRepository** - Manages chat messages
   - Get by ThreadId, Sender, Messages after ID
   - Latest message retrieval

6. **ChatThreadRepository** - Manages chat threads
   - Get open threads, threads needing agent
   - Get by client email, state, latest open thread

### Document Management
7. **DocumentoRepository** - Manages documents
   - Get by ExpedienteId, TipoDocumento, Estado
   - Date range filtering, documents with alerts

8. **ExpedienteRepository** - Manages case files
   - Get by PropiedadId, ClienteId, Estado, TipoOcupacion
   - Open expedientes, expedientes with documents

9. **AlertaVencimientoRepository** - Manages expiration alerts
   - Get by DocumentoId, Tipo, Estado
   - Upcoming alerts, overdue alerts

### Task Management
10. **TareaTramiteRepository** - Manages process tasks
    - Get by ExpedienteId, Estado, Prioridad
    - Overdue tasks, upcoming tasks, completed tasks

11. **CitumRepository** - Manages appointments
    - Get by ExpedienteId, Date range, Canal
    - Upcoming citas, past citas

### System Models
12. **RolRepository** - Manages roles
    - Get by name, roles with users

13. **UbicacionRepository** - Manages locations
    - Get by Provincia, Canton, Distrito
    - Locations with properties

14. **UsuarioRolRepository** - Manages user-role relationships
    - Get by UsuarioId, RolId, User/Role combination
    - User role validation

15. **AuditoriumRepository** - Manages audit logs
    - Get by UsuarioId, Accion, Entidad
    - Date range filtering, recent audits

## Base Repository Features

All repositories inherit from `RepositoryBase<T>` which provides:

### CRUD Operations
- `CreateAsync(T entity)` - Create new entity
- `ReadAsync()` - Get all entities
- `UpdateAsync(T entity)` - Update existing entity
- `UpdateManyAsync(IEnumerable<T> entities)` - Update multiple entities
- `DeleteAsync(T entity)` - Delete entity
- `FindAsync(int id)` - Find entity by ID
- `UpsertAsync(T entity, bool isUpdating)` - Insert or update
- `ExistsAsync(T entity)` - Check if entity exists

### Advanced Features
- **Async/Await** - All operations are asynchronous
- **Error Handling** - Try-catch blocks with proper exception handling
- **Entity Framework Integration** - Uses DbContext and DbSet
- **Include Support** - Repository implementations use Include() for related data
- **Filtering & Sorting** - Custom methods for specific business logic

## Usage Example

```csharp
// Register repositories in Program.cs
builder.Services.AddRepositories();

// Inject and use in your services
public class PropertyService
{
    private readonly IPropiedadRepository _propiedadRepository;
    
    public PropertyService(IPropiedadRepository propiedadRepository)
    {
        _propiedadRepository = propiedadRepository;
    }
    
    public async Task<IEnumerable<Propiedad>> GetActivePropertiesAsync()
    {
        return await _propiedadRepository.GetActivePropertiesAsync();
    }
}
```

## Dependency Injection

All repositories are registered in the DI container using the `AddRepositories()` extension method in `ServiceCollectionExtensions.cs`.

## Benefits

1. **Separation of Concerns** - Data access logic is isolated
2. **Testability** - Easy to mock repositories for unit testing
3. **Consistency** - All repositories follow the same pattern
4. **Maintainability** - Changes to data access logic are centralized
5. **Reusability** - Common operations are available across all repositories
6. **Performance** - Includes related data loading and optimized queries

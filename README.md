# Parte III - Ejercicios de Programación

Lenguaje utilizado en los tres ejercicios: **C# sobre .NET 10**.

## Estructura

- `src/FizzBuzz.Console`: Ejercicio 1, aplicación de consola.
- `src/ParteIII.Core`: Ejercicios 2 y 3, modelos, contratos y servicio.
- `tests/ParteIII.Core.Tests`: tests del ejercicio 3 con xUnit y Moq.

## Ejercicio 1 - FizzBuzz extendido

El programa recorre los números del 1 al 100. Primero comprueba los múltiplos de 18, que es el mínimo común múltiplo de 6 y 9, para evitar que esos números se clasifiquen solamente como `bio` o `metrika`.

Ejecución:

```powershell
dotnet run --project src/FizzBuzz.Console
```

## Ejercicio 2 - Análisis de código

### ¿Qué patrón implementa `ResultDto<T>`?

Implementa el patrón **Result** (resultado explícito). El objeto representa éxito o fallo y transporta el valor o los errores. Frente a las excepciones, hace que los fallos esperados del negocio sean visibles en el tipo de retorno, evita el costo y el flujo indirecto de lanzar/capturar excepciones, y obliga al consumidor a comprobar el resultado. Las excepciones siguen siendo apropiadas para fallos inesperados o técnicos.

### ¿Por qué se verifica `companyId` dos veces?

El repositorio filtra por empresa para no recuperar datos de otro tenant. El servicio vuelve a comprobar la propiedad porque aplica una defensa en profundidad y no confía ciegamente en que toda implementación del repositorio filtre correctamente. Esto previene acceso horizontal no autorizado entre empresas, conocido como **IDOR/BOLA**, causado por identificadores manipulados o por un repositorio defectuoso.

### ¿Qué hace `_mapper.Map<IdentityDto>()`?

Convierte la entidad de persistencia `Identity` en un `IdentityDto`, evitando exponer directamente la entidad y sus detalles internos. La librería usada típicamente en .NET es **AutoMapper**, incluida en esta solución.

### Mejoras aplicadas

- Se comprueba que `Value` no sea nulo incluso si un repositorio devuelve un resultado inconsistente.
- Se conserva la validación de empresa como defensa en profundidad.
- Se valida que el motivo de blacklist no sea vacío y se eliminan espacios externos.
- Se inyectan el repositorio y `IMapper` mediante el constructor.
- Se mantiene `ResultDto<T>` para errores esperados; el logging y las excepciones técnicas deberían manejarse en una capa global.
- En producción también conviene aceptar `CancellationToken` en toda la cadena asíncrona.

La versión mejorada se encuentra en `src/ParteIII.Core/Services/IdentityService.cs`.

## Ejercicio 3 - Tests unitarios

Los tests cubren identidad inexistente, identidad ya bloqueada, operación exitosa y motivo vacío. También verifican con Moq si `UpdateAsync` debe ejecutarse o no.

```powershell
dotnet test ParteIII.sln
```

## Publicación en GitHub

Desde la carpeta raíz:

```powershell
git init
git add .
git commit -m "Resolver ejercicios de programación Parte III"
git branch -M main
git remote add origin https://github.com/USUARIO/REPOSITORIO.git
git push -u origin main
```
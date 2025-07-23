# CleanArch

![GitHub last commit](https://img.shields.io/github/last-commit/gasbrieo/dotnet-cleanarch)
![Sonar Quality Gate](https://img.shields.io/sonar/quality_gate/gasbrieo_dotnet-cleanarch?server=https%3A%2F%2Fsonarcloud.io)
![Sonar Coverage](https://img.shields.io/sonar/coverage/gasbrieo_dotnet-cleanarch?server=https%3A%2F%2Fsonarcloud.io)
![NuGet](https://img.shields.io/nuget/v/Gasbrieo.CleanArch)

A lightweight **.NET library** providing **Clean Architecture building blocks** like a **Result pattern** and **CQRS abstractions**.

---

## ✨ Features

- ✅ **Result Pattern** for explicit success/failure handling
- ✅ **Standardized Error Types** (`Failure`, `Validation`, `NotFound`, `Conflict`)
- ✅ **CQRS Abstractions** (`ICommandHandler`, `IQueryHandler`)
- ✅ **CQRS Behaviors** (`LoggingBehavior`, `ValidationBehavior`)
- ✅ **Clean, dependency-free implementation**

---

## 🧱 Tech Stack

| Layer   | Stack                             |
| ------- | --------------------------------- |
| Runtime | .NET 9                            |
| Package | NuGet                             |
| CI/CD   | GitHub Actions + semantic-release |

---

## 📦 Installation

```bash
dotnet add package Gasbrieo.CleanArch
```

---

## 🚀 Usage

**Results**

```csharp
Result<string> user = Result<string>.Success("John Doe");

if (user.IsSuccess)
{
    Console.WriteLine(user.Value);
}
else
{
    Console.WriteLine(user.Error.Description);
}
```

**Messaging**

```csharp
public class CreateUserHandler : ICommandHandler<CreateUserCommand>
{
    public Task<Result> HandleAsync(CreateUserCommand command, CancellationToken ct)
    {
        // business logic...
        return Task.FromResult(Result.Success());
    }
}
```

---

## 🧱 Error Types & Usage

The library defines a **small, explicit set of error categories** to represent failures consistently across **domain**, **application**, and **infrastructure** layers.

Each `ErrorType` communicates **why** an operation failed, without assuming how it will be presented (HTTP, gRPC, messaging, etc.).

---

- **Validation** → the request contains **invalid or missing fields**, or violates a **domain validation rule**.  
  This category is intended for **multiple field-level errors** returned together.  
  _Example:_ email format is invalid, required fields are missing, or a business invariant is broken.

- **Problem** → a **known business rule** prevents the operation from succeeding, but it’s **not caused by invalid input**.  
  This category is intended for a **single, well-defined business error**.  
  _Example:_ attempting to deactivate the only remaining admin account, trying to process an order in a disallowed state.

- **NotFound** → the requested resource or entity **does not exist** or is **no longer available**.  
  _Example:_ fetching a user by an ID that does not exist, looking up a deleted record.

- **Conflict** → the operation is valid but **cannot proceed due to a conflicting state**.  
  _Example:_ trying to register a user with an email that already exists, attempting to update an entity modified concurrently.

- **Failure** → a **generic, unexpected error** that does not fit into any other category.  
  _Example:_ unhandled exception, infrastructure failure, or unknown error.

---

**Design intention:**  

- Use **Validation** when you need to return **multiple field errors at once**.  
- Use **Problem** when you need to return **a single business rule violation**.  

These error types are **transport-agnostic** – they describe the **reason for failure** without coupling to how the error will be returned in a specific protocol (e.g., HTTP or gRPC).

---

## 🔄 Releases & Versioning

This project uses **[semantic-release](https://semantic-release.gitbook.io/semantic-release/)** for fully automated versioning:

- **feat:** → minor version bump (0.x.0 → 0.(x+1).0)
- **fix:** → patch version bump (0.0.x → 0.0.(x+1))
- **feat!: / BREAKING CHANGE:** → major version bump (x.0.0 → (x+1).0.0)

Every merge into `main` automatically:

- Updates `CHANGELOG.md`
- Creates a GitHub release
- Publishes a new version to NuGet

See all changes in the [CHANGELOG.md](./CHANGELOG.md).

---

## 🪪 License

This project is licensed under the MIT License – see [LICENSE](LICENSE) for details.

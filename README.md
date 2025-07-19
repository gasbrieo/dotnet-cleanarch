# CleanArch

![GitHub last commit](https://img.shields.io/github/last-commit/gasbrieo/dotnet-cleanarch)
![Build](https://img.shields.io/github/actions/workflow/status/gasbrieo/dotnet-cleanarch/release.yml?branch=main)
![Sonar Quality Gate](https://img.shields.io/sonar/quality_gate/gasbrieo_dotnet-cleanarch?server=https%3A%2F%2Fsonarcloud.io)
![Sonar Coverage](https://img.shields.io/sonar/coverage/gasbrieo_dotnet-cleanarch?server=https%3A%2F%2Fsonarcloud.io)
![semantic-release](https://img.shields.io/badge/%20%20%F0%9F%93%A6%F0%9F%9A%80-semantic--release-e10079.svg)
![NuGet](https://img.shields.io/nuget/v/Gasbrieo.CleanArch)

A lightweight **.NET library** providing **Clean Architecture building blocks** like a **Result pattern** and **CQRS abstractions**.

---

## ✨ Features

- ✅ **Result Pattern** for explicit success/failure handling  
- ✅ **Standardized Error Types** (`Failure`, `Validation`, `NotFound`, `Conflict`)  
- ✅ **CQRS Abstractions** (`ICommandHandler`, `IQueryHandler`)  
- ✅ **Clean, dependency-free implementation**  
- ✅ **Multi-target** (`netstandard2.1` + `net8.0`)  

---

## 🧱 Tech Stack

| Layer   | Stack                             |
| ------- | --------------------------------- |
| Runtime | .NET 8 + .NET Std                 |
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
using Gasbrieo.CleanArch.Results;

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

The library provides a small set of **error categories** to model different failure scenarios consistently:

- **Business** → a business rule was violated or the requested action is not allowed in the current domain state  
  _Example:_ trying to cancel an already shipped order.  

- **Validation** → input data is invalid or inconsistent  
  _Example:_ email with invalid format, missing required fields.  

- **Conflict** → the requested action is valid but cannot proceed due to a conflicting state  
  _Example:_ trying to register a user with an email that already exists.  

- **NotFound** → the requested resource or entity does not exist  
  _Example:_ fetching a user by an ID that does not exist.  

- **Problem** → an unexpected or internal failure occurred, such as infrastructure errors  
  _Example:_ database timeout, external service unavailable.  

This categorization makes it easy to consistently handle errors across **application**, **domain**, and **infrastructure layers**, and later map them to any presentation layer (APIs, messaging, etc.) without ambiguity.

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

## 🧱 Project Structure

```
src/
├── CQRS/         # ICommandHandler, IQueryHandler
├── Results/      # Result<T>, Error, ErrorType
└── CleanArch.csproj
```

---

## 🪪 License

This project is licensed under the MIT License – see [LICENSE](LICENSE) for details.

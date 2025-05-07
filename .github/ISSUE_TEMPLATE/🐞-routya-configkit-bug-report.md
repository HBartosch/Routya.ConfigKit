---
name: "\U0001F41E Routya.ConfigKit Bug Report"
about: Report source generator bugs related to configuration binding
title: "[BUG]"
labels: bug
assignees: HBartosch

---

## 📌 Environment

- **.NET version**:
- **OS**:
- **Routya.ConfigKit  version**:
- **Build Tool**:
  - [ ] SDK-style project (.csproj)
  - [ ] IDE: Visual Studio / Rider / Other
- **Source Generator Behavior**:
  - [ ] No output generated
  - [ ] Invalid binding
  - [ ] Compilation error
- **Used in** (check one):
  - [ ] ASP.NET Core
  - [ ] Console App
  - [ ] Worker Service
  - [ ] Other: ____________

## 📝 Describe the Bug

_Explain what went wrong with config class generation._

## ✅ Expected Behavior

_Describe what classes or bindings should have been generated._

## ❌ Actual Behavior

_What actually happened? No output? Partial output?_

## 🔁 Steps to Reproduce

1. Define config class with attribute: `ConfigSection("MySection")`
2. Add `appsettings.json` section
3. Build solution
4. ...

## 📎 Code / Config Sample

```csharp
[ConfigSection("MySection")]
public class MyConfig
{
    public string Key { get; set; }
}
```

```json
"MySection": {
  "Key": "Value"
}
```
// stack trace here

## 🧪 Workarounds Tried
Examples: Clean build, rebuild, using dotnet build, checked Analyzer diagnostics, etc.

## 🙋 Additional Context
MSBuild version, IDE logs, or analyzer errors if any.

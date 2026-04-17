---
name: translate-to-ptbr
description: Translate English documentation files to Brazilian Portuguese.
---

# Skill Instructions

## Purpose
Translate English documentation files to Brazilian Portuguese.

## Supported targets
- README.md → README.pt-BR.md (root)
- CONTRIBUTING.md → docs/CONTRIBUTING-pt-BR.md

## Translation rules
1. Translate all prose, headings, and descriptions naturally to pt-BR
2. NEVER translate:
   - Code blocks (```bash, ```json, ```csharp, etc.)
   - CLI commands (dotnet run, git clone, chmod, etc.)
   - URLs and links
   - Environment variable names (JWT_SIGNING_KEY, etc.)
   - Package names, endpoint paths (/api/auth/login, etc.)
   - JSON keys
   - Branch naming conventions (feature/, fix/)
3. Preserve all emojis exactly as-is
4. Preserve all badges and image tags exactly as-is
5. Update the language switcher to highlight pt-BR as active and link back to English:
   `🇧🇷 Português | 🇺🇸 [English](README.md)`
6. Keep identical section order as the English source

## Output
Produce the complete translated file content ready to save.

```

---

### Step 4 — How to Invoke the Agent

Once set up, open Copilot Chat in VS Code and use natural language:
```
# Create the English README from scratch
@docs-agent create the README.md for this project

# Update after adding a new endpoint
@docs-agent update the Endpoints section in README.md to include the new Jobs endpoints

# Translate after any change
@docs-agent translate README.md to pt-BR

# Update CONTRIBUTING with a new dev rule
@docs-agent add a rule to CONTRIBUTING.md that DTOs must use records

# Full sync after a big change
@docs-agent update CONTRIBUTING.md with the new PR checklist and translate it to pt-BR
```

---

### How It All Fits Together
```
Developer invokes @docs-agent in Copilot Chat
          ↓
Agent reads .github/copilot-instructions.md (project context)
          ↓
Agent picks the right skill from .github/skills/
          ↓
Skill provides the rules and structure for the task
          ↓
Agent produces the file content
          ↓
Developer reviews, adjusts if needed, and saves the file
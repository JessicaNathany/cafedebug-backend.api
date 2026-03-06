---
name: manage-readme
description: Create or update README.md (English) for the Café Debug API
---

# Skill Instructions

## Purpose
Create or update README.md (English) for the Café Debug API.

## File target
- README.md (English, root)

## Required sections in order
1. Header with project name, badges and short description
2. Language switcher: `🇺🇸 English | 🇧🇷 [Português](README.pt-BR.md)`
3. About the project
4. Tech stack
5. Architecture diagram (preserve existing image links)
6. Prerequisites
7. Getting started (clone, database setup, appsettings, run)
8. Endpoints (Auth, BannerAdmin, Episodes — grouped by domain)
9. Tests
10. How to contribute (link to CONTRIBUTING.md)
11. License

## Rules
- Use English only
- Keep all existing badges, images, and links intact
- Never translate code blocks or commands
- Keep the language switcher banner always at the very top
- After updating README.md, remind the user to also run
  the translate-ptbr-skill to sync README.pt-BR.md
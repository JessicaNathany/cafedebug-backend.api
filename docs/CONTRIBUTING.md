# Contributing to Café Debug API ☕🚀

## About the project 📑
This project is the backend API for the Café Debug website. It’s currently running in production on ASP.NET Core MVC. The goal is to separate the frontend (FE) from the backend (BE) to better scale the site, build new pages and allow open‑source contributions.

The site includes an Admin module for managing episodes, banners, jobs and ads. New features will be described in issues or future documentation. The main screens are:

### Admin
- Episodes (CRUD)
- Banner (CRUD)
- Jobs (CRUD to manage positions, admin only)
- Team (CRUD for podcast members)
- Debuggers (CRUD for open‑source contributors)

### FE
**Current features:**
- Home (banner and latest 3 episodes)
- List episodes
- List Team members

**Upcoming features:**
- List books
- List debuggers
- List contributors (directly from GitHub)
- List job openings
- Translations (PT‑BR and ENG‑US)
- Search

---

## How to contribute 🤝

Pick up an issue on GitHub to develop a feature or fix a bug. Use the branch naming convention below:
- **feature:** `feature/feature-name`
- **bug:** `fix/bug-name`

Your PR will be reviewed to ensure standards and business requirements are met.

---

## Development guidelines 📝
- Do **not** use minimal APIs – follow the controller model.
- Use English throughout.
- Choose clear, verbose names.
- Avoid unnecessary comments (except for business rules).
- Prefer simple solutions; avoid unnecessary complexity.
- Add unit tests for new features/changes.
- Don’t install redundant libraries (e.g. if you use xUnit, don’t add NSubstitute without justification).
- Notify before modifying the `debug-automation` project.
- Suggestions for features/libraries should be opened as issues.
- Use **records** for DTO classes such as Requests and Responses.
- Employ rich domain models for entities; encapsulate business logic when needed.
- Never expose sensitive data in code or logs.
- Keep dependencies up‑to‑date and remove unused ones.
- Follow semantic versioning for releases.
- Be respectful and open to everyone’s suggestions.
- Handle errors consistently and securely.
- Test locally before submitting a PR.

---

## Submitting your PR 📬

When opening a PR, include in the description:
- **Feature/Bug:** A description of the feature or bug
- **Description:** Details of the changes made

---

## Running the project locally 🖥️

1. Clone the [debug-automation](https://github.com/JessicaNathany/debug-automation) project to spin up the local database and test environments. Use the user `debugcafe@local.com`. Configure Postman with the `cafe-collection.json` file at the repo root.

2. Make the script executable:
   ```bash
   chmod +x cafedebug-setup.sh
   ```

3. Run the database script:
   ```bash
   ./cafedebug-setup.sh
   ```

   ![screenshot](https://github.com/user-attachments/assets/5b8949f0-ad8c-49f3-b2a4-f389950c3b5a)

   ![screenshot](https://github.com/user-attachments/assets/defd3d75-288b-400a-aa92-1d7ca7a9a5b4)

4. The project targets .NET 9.
   **Windows (Visual Studio):** just run it normally.
   **Linux/Mac:**
   1. Install .NET SDK 9
   2. Clone the backend API:
      ```bash
      git clone https://github.com/JessicaNathany/cafedebug-backend.api.git
      ```
   3. Restore packages:
      ```bash
      dotnet restore
      ```
   4. Run the application:
      ```bash
      dotnet run --project src/cafedebug-backend.api/cafedebug-backend.api.csproj
      ```

---
## Example environment variable configuration

### For Visual Studio (Windows)
Add to your `Properties/launchSettings.json`:

```json
{
  "profiles": {
    "cafedebug-backend.api": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "launchUrl": "swagger",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "ConnectionStrings__CafedebugConnectionStringMySQL": "server=localhost;port=3307;database=cafedebug-mysql-local;user=root;password=root; sslmode=none",
        "SMTP_SERVER": "smtp.gmail.com",
        "SMTP_NAME": "Café Debug - Fale conosco",
        "SMTP_PORT": "587",
        "SMTP_PASSWORD": "123465",
        "SMTP_FROM": "faleconosco.cafedebug@gmail.com",
        "AWSS3_BUCKETNAME": "cafedebug-uploads",
        "AWS_ACCESS_KEY_ID": "123465abc",
        "AWS_SECRET_KEY": "123abc",
        "FORGOT_PASSWORD_URL": "http://www.cafedebug.com.br/forgot-password"
      },
      "applicationUrl": "http://localhost:46370"
    }
  }
}
```

### For Mac/Linux
Export before running the project:

```bash
export ASPNETCORE_ENVIRONMENT=Development
export ConnectionStrings__CafedebugConnectionStringMySQL="server=localhost;port=3307;database=cafedebug-mysql-local;user=root;password=root; sslmode=none"
export SMTP_SERVER="smtp.gmail.com"
export SMTP_NAME="Café Debug - Fale conosco"
export SMTP_PORT="587"
export SMTP_PASSWORD="123465"
export SMTP_FROM="faleconosco.cafedebug@gmail.com"
export AWSS3_BUCKETNAME="cafedebug-uploads"
export AWS_ACCESS_KEY_ID="123465abc"
export AWS_SECRET_KEY="123abc"
export FORGOT_PASSWORD_URL="http://www.cafedebug.com.br/forgot-password"
```

> Adjust values according to your environment.

If you have questions, open an issue! Happy contributing! 🚀

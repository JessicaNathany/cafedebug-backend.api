# Contributing to Café Debug API ☕🚀

## Sobre o projeto 📑
Esse projeto é a API backend do site Café Debug. Atualmente está em produção na arquitetura ASP.NET Core MVC. O objetivo é separar o frontend (FE) do backend (BE) para escalar melhor o site, criar novas telas e permitir contribuições open source.

O site possui um módulo de Admin para cadastro de episódios, banners, vagas e publicidades. Novas features serão descritas em issues ou documentações futuras. As principais telas são:

### Admin
- Episódios (CRUD)
- Banner (CRUD)
- Jobs (CRUD para gerenciar vagas, apenas Admin)
- Team (CRUD dos membros do podcast)
- Debuggers (CRUD de contribuidores open-source)

### FE
**Funcionalidades atuais:**
- Home (banner e últimos 3 episódios)
- Listar episódios
- Listar Team (membros do podcast)

**Funcionalidades novas:**
- Listar livros
- Listar debuggers
- Lista de contribuidores (direto do GitHub)
- Lista de vagas
- Traduções (PT-BR e ENG-US)
- Pesquisa

---

## Como contribuir 🤝

Você pode pegar uma issue no GitHub para desenvolver uma feature ou corrigir um bug. Use o padrão de branch abaixo:
- **feature:** `feature/nome-funcionalidade`
- **bug:** `fix/nome-bug`

O PR passará por revisão para garantir padrões e requisitos de negócio.

---

## Padrões de desenvolvimento 📝
- Não utilize minimal API: siga o modelo de controller.
- Utilize o idioma inglês.
- Nomes verbosos e claros.
- Evite comentários desnecessários (exceto regras de negócio).
- Prefira soluções simples, evite complexidade desnecessária.
- Crie testes de unidade para novas funcionalidades/modificações.
- Evite bibliotecas redundantes (ex: se usa XUnit, não instale NSubstitute sem justificativa).
- Notifique antes de modificar o projeto debug-automation.
- Sugestões de funcionalidades/bibliotecas devem ser abertas como issue.
- Utilize *records* para classes *DTOs* como Request e Response
- Utilize domínios ricos para as entidades, encapsule a lógica de negócio quando necessário
- Nunca exponah dados sensíveis no código ou nos logs
- Atualize dependências regularmente e remova as não utilizadas
- Siga versionamento semântico para releases
- Seja respeitoso nas interações e aberto a sugestões de todos os colaboradores
- Trate erros de forma consistente e segura
- Teste localmente antes de submeter PR.

---

## Submetendo seu PR 📬

Ao abrir um PR, inclua na descrição:
- **Feature/Bug:** Descrição da feature ou bug
- **Descrição:** Detalhe as mudanças realizadas

---

## Executando o projeto localmente 🖥️

1. Clone o projeto [debug-automation](https://github.com/JessicaNathany/debug-automation) para subir o banco local e ambientes de teste. Use o usuário `debugcafe@local.com`. Configure o Postman com o arquivo `cafe-collection.json` na raiz.

2. Dê permissão ao script:
```bash
chmod +x cafedebug-setup.sh
```
3. Execute o script do banco:
```bash
./cafedebug-setup.sh
```

4. O projeto está na versão .NET 9. Para rodar no Visual Studio (Windows), basta executar normalmente. Para Linux/Mac:
   1. Instale o .NET SDK 9
   2. Clone o backend-api:
      ```bash
      git clone https://github.com/JessicaNathany/cafedebug-backend.api.git
      ```
   3. Restaure os pacotes:
      ```bash
      dotnet restore
      ```
   4. Execute a aplicação:
      ```bash
      dotnet run --project src/cafedebug-backend.api/cafedebug-backend.api.csproj
      ```

---

Se tiver dúvidas, abra uma issue! Boas contribuições! 🚀

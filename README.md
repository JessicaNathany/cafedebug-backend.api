# cafedebug-backend.api

![image](https://user-images.githubusercontent.com/11943572/234849730-c6b41618-6c13-4a87-9b5e-5b9d16ba4474.png)


<p align="center">
  <img src="https://img.shields.io/badge/Framework-dotnet-blue"/> 
  <img src="https://img.shields.io/badge/Framework%20version-dotnet%206-blue"/>
  <img src="https://img.shields.io/badge/Language-C%23-blue"/> 
  <img src="https://img.shields.io/badge/Status-development-green"/>  
   <img src=" https://img.shields.io/badge/Status-development-green"/>  
</p>

 <h4 align="center"> 
	🚧  Projeto 🚀 em construção...  🚧
 </h4>

 ## Sobre o projeto 📑
 
 Este é o repositório do projeto API Café Debug. Essa API tem como objetivo manter o backend separado do frontend<br/>
 trazendo informações do podcast como episódios e agenda e outros conteúdos relacionado a tecnologia. O site atual está
 sendo utilizado ASP.NET Core MVC, legado (wwww.cafedebug.com.br).


## Setup 🔧

## Tests 🧪
Para conseguir rodar a base local, faça um [clone deste projeto](https://github.com/JessicaNathany/debug-automation) e execute o comando abaixo:

Dê permissão ao arquivo .sh

```bash
 chmod +x cafedebug-setup.sh
```
- Execute o comando para rodar o script do banco de dados.

```bash
 ./cafedebug-setup.sh
```

## Endpoints :clipboard: <br/>

*AdvertisementsAdmin*
- `GET /listar-anuncios` - retorna uma lista de anúncios.
- `GET /listar-anuncios/{id}` - retorna um anúncio específico por ID.
- `POST /novo-anuncio` - adiciona um novo anúncio.
- `PUT /editar-anuncio/{id}` - edita um anúncio.

*Auth*
- `POST /api/auth/login` - autenticação do usuário retornando um token de validação.

*BannerAdmin*
- `POST /api/banner-admin/novo-banner` - adiciona um novo banner aa área administrativa.
- `PUT /api/banner-admin/editar-banner` -  edita o banner da área administrativa.
- `GET /api/banner-admin/banners`  - retorna uma lilsta de banners da área administrativa.
- `GET /api/banner-admin/banner/{id}`  - retorna banner por id.
- `DELETE /api/banner-admin/banner/{id}` - apaga banner por id.

*EpisodeAdmin*
- `POST /api/episodio-admin/novo-episodio` - adiciona um novo episódio aa área administrativa.
- `PUT /api/episodio-admin/editar-episodio` -  edita o episódio da área administrativa.
- `GET /api/episodio-admin/episodio`  - retorna uma lilsta de episódios da área administrativa.
- `GET /api/episodio-admin/episodio/{id}`  - retorna episódio por id.
- `DELETE /api/episodio-admin/episodio/{id}` - apaga episódio por id.







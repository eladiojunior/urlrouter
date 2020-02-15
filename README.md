# UrlRouter
Projeto para realizar o roteamento de URLs realizando o controle de acesso e configuração para diferentes rotas a depender do sistema operacional do dispositivo móvel que está realizando o acesso.

## Estrutura do Projeto
Desenvolvido em .NET C# Core, com dois projetos:
- UrlRouter.WebApi: Responsável pelo acesso ao repositório de dados, em MongoDB no container;
- UrlRouter.AspNetMvc: Responsável pela interfaces de manutenção das rotas e configuração e disponibilização do acesso a URL, via Action;

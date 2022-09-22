## Sobre
Esse é um projeto desenvolvido como template para você criar comandos de forma escalar.

## Configuração
Localize o `appsettings.json` e adicione um novo comando seguindo a estrutura
```
{
  "CommandName": "help",
  "Description": "Use para listar todos comandos disponíveis na aplicação.",
  "MethodName": "CommandHelp",
  "Params": [
    {
      "ParamName": "f",
      "Description": "Descrição completa"
    }
  ]
}
```

Os campos devem ser preenchidos com as seguintes informações:

- CommandName - Defina o comando a ser digitado.
- Description - A descrição do comando para ser exibida no comando "help".
- MethodName - O nome do método interno a ser invocado.
- Params.ParamName - Defina a sigla do parâmetro.
- Params.Description - Defina a descrição do parâmetro.

Após isso, basta configurar o método público na class `Program.cs` com o nome configurado acima para receber os parâmetros quando enviado ao console:

```
public static string CommandHelp(params Param[] parameters)
```

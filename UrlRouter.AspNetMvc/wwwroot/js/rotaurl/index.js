Index = {
    InitLista: function () {
        $("button.filtrar").click(function () {
            Index.CarregarLista();
        });
        $("button.registrar").click(function () {
            Global.SubmitActionGet("Novo", "RotaUrl");
        });
    },
    CarregarLista: function () {
        Global.MostrarLoading(true);
        $.ajax({
            cache: false,
            type: "POST",
            url: urlRoot + "/RotaUrl/Listar",
            dataType: "json",
            data: $("form.form-filtro").serialize(),
            success: function (result) {
                Global.MostrarLoading(false);
                if (result.hasErro) {
                    Global.ExibirMensagem(result.erros, true);
                    return;
                }
                $("div.lista").html(result.model);
                $("a.editar").click(function () {
                    var id = $(this).data("id");
                    Global.SubmitActionPost("CarregarEdicao", "RotaUrl", { "chaveRota": id });
                });
            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                Global.MostrarLoading(false);
                Global.ExibirMensagem('Erro na solicitação.', true);
            }
        });
    }
};
$(function () {
    Index.InitLista();
    Index.CarregarLista();
});
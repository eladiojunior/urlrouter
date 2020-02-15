Global = {

    ApresentarProcessandoAjaxRequest: function () {
        $(document).on("ajaxStart", function () {
            processando = true;
            window.setTimeout(function () {
                if ($.active !== 0) {
                    Global.MostrarLoading(true);
                }
            }, 200);
        });
        $('form').submit(function () {
            Global.MostrarLoading(true);
        });
    },

    RemoverProcessandoFimAjaxRequest: function () {
        $(document).on("ajaxStop", function () {
            processando = true;
            Global.MostrarLoading(false);
        });
    },

    LimparDropbox: function (dropbox, textOptionDefault, valueOptionDefault) {
        dropbox.find('option')
            .remove().end()
            .append("<option value='" + valueOptionDefault + "'>" + textOptionDefault + "</option>")
            .val(valueOptionDefault);
    },

    CarregarDropbox: function (dropbox, listOptionDropbox, textOptionDefault, valueOptionDefault) {
        dropbox.find('option').remove().end()
            .append("<option value='" + valueOptionDefault + "'>" + textOptionDefault + "</option>").val(valueOptionDefault);
        listOptionDropbox.forEach(function (item) {
            dropbox.append($('<option>', {
                value: item.ValueDropbox,
                text: item.TextDropbox
            }));
        });
    },

    MostrarLoading: function (mostrar) {
        if (mostrar) {
            $("div.loading").removeClass("d-none");
            $("button[type=submit]", this).attr("disabled", "disabled");
            $("input[type=submit]", this).attr("disabled", "disabled");
        } else {
            $("div.loading").addClass("d-none");
            $("button[type=submit]").removeAttr("disabled");
            $("input[type=submit]").removeAttr("disabled");
        }
    },

    LimparMensagem: function () {
        $(".mensagens").hide();
    },

    ExibirMensagem: function (msg, hasErro) {

        var alert = $(".mensagens .alert");
        if (alert.length > 0) {
            var msgAlert = msg;
            if (Object.prototype.toString.call(msg) === '[object Array]') {
                msgAlert = '';
                for (var i = 0; i < msg.length; i++) {
                    if (msgAlert !== '') msgAlert += '<br/>';
                    msgAlert += msg[i];
                }
            }
            alert.html(msgAlert);
            alert.removeClass("alert-danger").removeClass("alert-info");
            alert.addClass(hasErro ? "alert-danger" : "alert-info");
            $(".mensagens").show();
            $("html,body").scrollTop(0);
        }
    },

    ExibirConfirmacao: function (titulo, mensagem, callbackSim, callbackNao, idModal) {
        if (idModal === null || idModal.trim() === '') { idModal = "ModalConfirmacao"; }
        var objModelConfirmacao = $("#" + idModal).attr("id");
        if (objModelConfirmacao) {//Existe objeto Model de confirmação... reutilizar.
            $("#titulo" + idModal).val(titulo);
            $("#mensagem" + idModal).val(mensagem);
        }
        else {//Criar um objeto novo, no BODY!
            var htmlModel = "";
            if (titulo === null || titulo.trim() === '') { titulo = "Confirmação"; }
            if (mensagem === null || mensagem.trim() === '') { mensagem = "Por favor, confirme a operação?"; }
            htmlModel += "<div id=\"" + idModal + "\" class=\"modal fade\" tabindex=\"-1\" role=\"dialog\" data-backdrop=\"static\"  aria-labelledby=\"" + idModal + "\">";
            htmlModel += "  <div class=\"modal-dialog\" role=\"document\">";
            htmlModel += "    <div class=\"modal-content\">";
            htmlModel += "      <div class=\"modal-header\">";
            htmlModel += "        <h4 class=\"modal-title\" id=\"titulo" + idModal + "\">" + titulo + "</h4>";
            htmlModel += "      </div>";
            htmlModel += "      <div class=\"modal-body\">";
            htmlModel += "        <p id=\"mensagem" + idModal + "\">" + mensagem + "</p>";
            htmlModel += "      </div>";
            htmlModel += "      <div class=\"modal-footer\">";
            htmlModel += "        <button type=\"button\" class=\"btn btn-primary\" id=\"btnSim" + idModal + "\" data-dismiss=\"modal\">Sim</button>";
            htmlModel += "        <button type=\"button\" class=\"btn btn-default\" id=\"btnNao" + idModal + "\" data-dismiss=\"modal\">Não</button>";
            htmlModel += "      </div>";
            htmlModel += "    </div>";
            htmlModel += "  </div>";
            htmlModel += "</div>";
            $("body").append(htmlModel);
            if ($.isFunction(callbackSim))
                $("#btnSim" + idModal).click(callbackSim);
            if ($.isFunction(callbackNao))
                $("#btnNao" + idModal).click(callbackNao);
        }
        $("#" + idModal).modal();
    },

    SubmitActionPost: function (actionName, controllerName, parametros) {
        var form = document.createElement("form");
        form.method = "POST";
        form.action = urlRoot + "/" + controllerName + "/" + actionName;
        $.map(parametros, function (value, key) {
            var elementInput = document.createElement("input");
            elementInput.type = "hidden";
            elementInput.name = key;
            elementInput.value = value;
            form.appendChild(elementInput);
        });
        document.body.appendChild(form);
        form.submit();
    },
    SubmitActionGet: function (actionName, controllerName, parametros) {
        var form = document.createElement("form");
        form.method = "GET";
        form.action = urlRoot + "/" + controllerName + "/" + actionName;
        $.map(parametros, function (value, key) {
            var elementInput = document.createElement("input");
            elementInput.type = "hidden";
            elementInput.name = key;
            elementInput.value = value;
            form.appendChild(elementInput);
        });
        document.body.appendChild(form);
        form.submit();
    },
    LimparCampos: function (form) {
        if (form !== null) {
            form.find("input[type=text]").val('');
        }
    }
};

$(function () {
    Global.ApresentarProcessandoAjaxRequest();
    Global.RemoverProcessandoFimAjaxRequest();
    Global.LimparMensagem();
});
var getPasswordModal = document.getElementById("savePasswordModal");

function DisableEditingCharacter(value) {
    if (value) {
        $('.need-password').attr("disabled", "disabled");
        $('#addRelationshipBtn').hide();
        $('#uploadImage').hide();
        $('#editMother').hide();
        $('#editFather').hide();
        $('input:submit').hide();
    }
    else {
        $('.need-password').attr("disabled", false);
        $('#addRelationshipBtn').show();
        $('#uploadImage').show();
        $('#editMother').show();
        $('#editFather').show();
        $('input:submit').show();
    }
};

function DisableDeleteCharacter(value) {
    if (value) {
        $('.btn-delete-char').hide();
    }
    else {
        $('.btn-delete-char').show();
    }
};

$(function () {
    $('#addNewCharacter').click(function () {
        document.getElementById("confirmPasswordBtn").onclick = function () { AddNewCharView(); };
        getPasswordModal.style.display = "block";
    });
});

function AddNewCharView() {
    if (IsPasswordCorrect() == true) {
        window.location.href = "Home/CreateNewCharacter";
    }
}

$(function () {
    $('#deleteAnyCharacter').click(function () {
        document.getElementById("confirmPasswordBtn").onclick = function () { Delete(); };
        getPasswordModal.style.display = "block";
    });
});


function Delete() {
    if (IsPasswordCorrect() == true) {
        DisableDeleteCharacter(false);
        getPasswordModal.style.display = "none";
    }
}

$(function () {
    $('#edit-character').click(function () {
        document.getElementById("confirmPasswordBtn").onclick = function () { EnabledEdit(); };
        getPasswordModal.style.display = "block";
    });
});

function EnabledEdit() {
    if (IsPasswordCorrect() == true) {
        DisableEditingCharacter(false);
        $('#edit').replaceWith('<div id="save">' +
            '<input type="submit" value="Zapisz" onclick="ClearTemporaryJSArrays()" class="blue-btn save-btn" />' +
            '</div>');
        getPasswordModal.style.display = "none";
    }
}

function IsPasswordCorrect() {
    var result = false;
    $.ajax({
        type: 'GET',
        async: false,
        url: '/Home/IsPasswordCorrect',
        data: { password: $('#Password').val() },
        success: function (data) {
            result = data;
        },
    });
    return result;
};
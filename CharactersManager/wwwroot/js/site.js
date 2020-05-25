// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


var relationships = [];
var addRelationshipModal = document.getElementById("addModal");
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

function ReloadRelationships(characterId, relationshipCharName, relationshipType) {  
    var newRelationship = [];

    if (relationshipCharName !== null && relationshipType !== null) {

        $.ajax({
            type: 'GET',
            url: '/Home/Exist',
            data: {relationshipCharacterName: relationshipCharName },
            success: function (id) {
                if (id) {
                    newRelationship.push("<a href='/Home/CharacterView/" + id + "'>" + relationshipCharName + " - " + relationshipType + "</a><br>");
                }
                else {
                     newRelationship.push("<a>" + relationshipCharName + " - " + relationshipType + "</a><br>");
                }
                $('#relationships').append(newRelationship);
            },
            
        });

        $.ajax({
            type: 'POST',
            url: '/Home/AddRelationship',
            data: { characterId: characterId, relationshipCharacterName: relationshipCharName, relationship: relationshipType }
        });
    }      
   
    CloseAddRelationshipModal();    
};

function LoadRelationships(characterId) {
    $('#relationships').empty();

    $.getJSON("/Home/GetRelationhips/" + characterId, function (result) {

        $.each(result, function () {

            $.ajax({
                type: 'GET',
                url: '/Home/Exist',
                data: { relationshipCharacterName: this.targetRelationshipCharacter.name + ' ' + this.targetRelationshipCharacter.surname },
                success: function (id) {
                    if (id !== 0) {
                        relationships.push("<a href='/Home/CharacterView/" + id + "'>" + this.targetRelationshipCharacter.name + "-" + this.type + "</a><br>");
                    }
                    else {
                        relationships.push("<a>" + this.targetRelationshipCharacter.name + "-" + this.type + "</a><br>");
                    }
                },

            });           
        });

        $('#relationships').append(relationships.join(""));
    });
};

function ClearTemporaryJSArrays() {   
    relationships = [];   
};

function CloseAddRelationshipModal() {
    $('#Characters').val("");
    $('#Relationship').val("");
    addRelationshipModal.style.display = "none";
};

window.onclick = function (event) {
    if (event.target == addRelationshipModal) {
        addRelationshipModal.style.display = "none";
        $('#Characters').val("");
        $('#Relationship').val("");
    }
}

$(function () {
    $('#addRelationshipBtn').click(function () {
        addRelationshipModal.style.display = "block";
    });
});

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

$(function () {
    $('#close-add-relation').click(function () {
        addRelationshipModal.style.display = "none";
    });
});

$(function () {
    $('#close-get-pass').click(function () {
        getPasswordModal.style.display = "none";
    });
});

$(function () {
    $('#editMother').click(function () {
        document.getElementById('mother').innerHTML = document.getElementById('motherEdit').innerHTML;
    });
});

$(function () {
    $('#editFather').click(function () {
        document.getElementById('father').innerHTML = document.getElementById('fatherEdit').innerHTML;
    });   
});

$(function () {
    $('#Filter').click(function () {
        window.location.href = "/Home/Filter/" + $('#Breed').val();
    });
});

$(function () {
    $.getJSON("/Home/GetAllCharacters/", function (result) {

        $.each(result, function () {
            $('#charactersList').append("<option value='" +
                this + "'></option>");
        });       
    });       
});

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




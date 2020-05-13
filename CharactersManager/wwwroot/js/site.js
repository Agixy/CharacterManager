// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


var relationships = [];
var addRelationshipModal = document.getElementById("addModal");
var getPasswordModal = document.getElementById("savePasswordModal");

DisableAllInputs();

function DisableAllInputs() {
    $('.need-password').attr("disabled", "disabled");
    //$('#addRelationshipBtn').hide();
    $('#editMother').hide();
    $('#editFather').hide(); 
    $('.btn-delete-char').hide()    
    
    $('input:submit').hide();
};

function EnableAllInputs() {
    $('.need-password').attr("disabled", "");
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
        getPasswordModal.style.display = "block";
    });
});

$(function () {
    $('#deleteAnyCharacter').click(function () {
        getPasswordModal.style.display = "block";
    });
});

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
        window.location.href = "/Home/Filter/" + $('#Breed').val() + "/" + 4;
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

$(document).ready(function () {
    $('#confirm-password').click(function () {
        $.ajax({            
            type: 'GET',
            url: '/Home/IsPasswordCorrect',
            data: { password: $('#Password').val() },
            success: function (isPasswordCorrect) {
                if (isPasswordCorrect) {                    
                    window.location.assign("/Home/CreateNewCharacter");
                }               
            },
        })
    });
   
});
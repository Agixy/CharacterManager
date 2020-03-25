// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


var relationships = [];
var addRelationshipModal = document.getElementById("addModal");
var closeButtonsArray = document.getElementsByClassName("close");

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

Array.prototype.forEach.call(closeButtonsArray, function (element) {
    element.onclick = function () {
        addRelationshipModal.style.display = "none";
        $("#addModal")
            .find("input[type=text],select")
            .val('')
            .end();            
    }
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

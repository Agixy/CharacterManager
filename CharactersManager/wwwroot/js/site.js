// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


var relationships = [];
var addRelationshipModal = document.getElementById("addModal");
var closeButtonsArray = document.getElementsByClassName("close");

function ReloadRelationships(characterId, relationshipCharId, relationshipCharName, relationshipType) {  
    var newRelationship = [];

    if (relationshipCharId !== null && relationshipType !== null) {
        newRelationship.push("<a href='/Home/CharacterView/" + relationshipCharId + "'>" + relationshipCharName + " - " + relationshipType + "</a><br>");

            $.ajax({
                type: 'POST',
                url: '/Home/AddRelationship',
                data: { characterId: characterId, relationshipCharacterId: relationshipCharId, relationship: relationshipType }
            });
    }      

    $('#relationships').append(newRelationship);
    CloseAddRelationshipModal();    
};

function LoadRelationships(characterId) {
    $('#relationships').empty();

    $.getJSON("/Home/GetRelationhips/" + characterId, function (result) {

        $.each(result, function () {
            relationships.push("<a href='/Home/CharacterView/" + this.targetRelationshipCharacter.id + "'>" + this.targetRelationshipCharacter.name + "-" + this.type + "</a><br>");
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


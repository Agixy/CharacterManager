
var relationships = [];
var addRelationshipModal = document.getElementById("addModal");


function ReloadRelationships(characterId, relationshipCharName, relationshipType) {
    var newRelationship = [];

    if (relationshipCharName !== null && relationshipType !== null) {

        $.ajax({
            type: 'GET',
            url: '/Home/Exist',
            data: { relationshipCharacterName: relationshipCharName },
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

    $('#Characters').val("");
    $('#Relationship').val("");
    addRelationshipModal.style.display = "none";
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
    $.getJSON("/Home/GetAllCharacters/", function (result) {

        $.each(result, function () {
            $('#charactersList').append("<option value='" +
                this + "'></option>");
        });
    });
});






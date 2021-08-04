function addField() {
    var addedFieldCount = $("[id^=performer-name]").length;
    if (addedFieldCount == 3) {
        alert('No more performers allowed');
        return;
    }
    $('#performersFieldContainer').append('<input required id="performer-name" onchange="getPerformerIdByName(this)"/>');
}
function postToFacebook() {
    var facebookMessage = $('#facebookMessageInput').val();
    const FacebookPageId = "100168142240956";
    const FacebookPageToken = "EAA3O6dvVFWYBAMFwlbfrcBwXMURph0ELaZA2rKZCOrZBUt6T6AFWMybMEI9UWAVY0IxNGCOBIlmcKfZAFNp4XNY58uDhJK8Yz4k20WsLOJlBepYikJZA5rUZCfjhSdCOMO0qdBlHCElSSbTM9ao9UNmNcKi8TtaWdaHY6HsDATOST8M2gGy4Xw";
    const FacebookApi = "https://graph.facebook.com/";
    const postReqUrl = FacebookApi + FacebookPageId + "/feed?message=" + facebookMessage + "&access_token=" + FacebookPageToken;
    if (facebookMessage) {
        $.ajax({
            url: postReqUrl,
            type: "POST",
            success: function (data, textStatus, jqXHR) {},
            error: function (jqXHR, textStatus, errorThrown) {
                alert(textStatus + " " + errorThrown);
            }
        });
    }
}
function getPerformerIdByName(item) {
    var queryParams = item.value;
    if (queryParams) {
        $.ajax({
            url: 'GetArtistIdBySearchParams/',
            type: "GET",
            data: {
                queryParams: queryParams,
            },
            success: function (result) {
                if (result === 'NO_RESULT') {
                    alert("No artist found with this name");
                }
                else {
                    var inputElement = document.createElement("input")
                    inputElement.type = "hidden";
                    inputElement.id = "performer-id";
                    inputElement.name = "performersId";
                    inputElement.value = result;
                    $('#performersFieldContainer').append(inputElement);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(textStatus + " " + errorThrown);
            }
        });
    }
}

var GuserID = "";
function ParseJsonData(data) {
    var itemsArr = data.items;
    console.log(data.toString());
    for (var x = 0 ; x < itemsArr.length; x++) {
        var obj = itemsArr[x];
        console.log(obj.url + '\n');
        
        var publishedDate = new Date(obj.published);
        var updateDate = new Date(obj.updated);
       
        console.log("publlished at " + publishedDate + "       ");
        
        console.log(" last updated at " + updateDate + "\n");
        //-----------------------------------------------------------
        console.log(" \n ------------------------------------\n"
            + "user who shred the data info  : \n");
        var actorObj = obj.actor;
        var userID = actorObj.id;
        
        var userName = actorObj.displayName;
        var accountUrl = actorObj.url;
        var userImage = actorObj.image.url;

        var accessDesc = obj.access.description;

        var objectObj = obj.object;
        var attachmentArr = objectObj.attachments;
        var postType = attachmentArr[0].objectType;
        var contentTitle = attachmentArr[0].displayName;
        var contentUrl = attachmentArr[0].url;

        

        var design = "  <div class=\"gcontainer\"> " +
            "<div class=\"header\">" +
            "<div> <a href=\"" +
            accountUrl +
            "\">" +
            "<img src=\"" +
            userImage +
            "\">" +
            "</a><h2>" +
            userName +
            "</h2>" +
            "<h3>" +
            accessDesc +
            "</h3>" +
            "<a class=\"time\" href=\"#\">" +
            publishedDate +
            "</a></div></div>" +
            "<div class=\"content\">" +
            "<p>" +
            contentTitle +
            "</p>";
            
        console.log("actor id  " + userID + "     ,");
        console.log("displayName : " + userName + "     ,");
        console.log("accout url : " + accountUrl + "     ,");
        console.log("image url : " + userImage + "\n\n");
        //----------------------------------------------------------

        //public
        
        //----------------------------------------------------------
        console.log("  object that hold the post data \n");
        
        console.log("post type  " + postType + "     ,");
        console.log("post displayName  " + contentTitle + "     ,");
        
        
        if (attachmentArr[0].image != undefined) {
            var imageFromAtt = attachmentArr[0].image;
            var imageUrl = imageFromAtt.url
            design += "</div><div class=\"image\">"+
                "<a href=\"#\">"+
                "<img src=\"\">"+
            "<img src=\""+imageUrl+"\"></a></div></div>";
            console.log(imageUrl  + "  , height : " + imageFromAtt.height + "   , width : " + imageFromAtt.width +
                "  ,  type : " + imageFromAtt.type);
        }
        else {
            console.log("array not exist ");
        }
        if (attachmentArr[0].fullImage != undefined) {
            console.log("full image :   " + attachmentArr[0].fullImage.url);
        }
        document.getElementById("design").innerHTML += design;
        console.log("\n\n\n\n");

    }

    //    
    //  $.each( data, function( key, val ) {
    //    items.push( "<li id='" + key + "'>" + val + "</li>" );
    //  });
    // 
    //  $( "<ul/>", {
    //    "class": "my-new-list",
    //    html: items.join( "" )
    //  }).appendTo( "body" );
}

function getPosts() {
    var request = gapi.client.plus.activities.list({
        'userId': 'me',
        'collection': 'public'
    });


    request.execute(function (resp) {

        // ParseJsonData(resp);
         alert("GuserID : " + GuserID);
            $.get("https://www.googleapis.com/plus/v1/people/"+GuserID+"/activities/public?key=AIzaSyD_oNG0Jw7zI0q8TJNRd0WphZbabUTXOPc"
                , function (data, status) {
                    ParseJsonData(data);
                alert("Data: " + data + "\nStatus: " + status);
            });
        //    var numItems = resp.items.length;
        //    for (var i = 0; i < numItems; i++) {
        //        //console.log('ID: ' + resp.items[i].id + ' Content: ' +
        //        //  resp.items[i].object.content);

        //        var request = gapi.client.plus.activities.get({ 'activityId': resp.items[i].id });

        //        request.execute(function (resp2) {
        //            console.log('Used get to fetch activity with ID: ' + resp2.id);
        //            /*
        //            https://www.googleapis.com/plus/v1/people
        //            user id --> /108189587050871927619/
        //            activities/public
        //             api key --> ?key=AIzaSyCxfI-eFJXz25cuT6sJUg5y_PrBUQnC0_A
        //            post id &activityid=108189587050871927619
        //            **/

        //            var link = "https://www.googleapis.com/plus/v1/people/" +
        //                /*  document.cookie.split('; ')[0];*/"116121647441891070688"
        //                + "/activities/public?key=AIzaSyCxfI-eFJXz25cuT6sJUg5y_PrBUQnC0_A"
        //                + "&activityid=" + resp2.id;
        //            console.log(link);

        //            $.get(link, function (data, status) {


        //                /*     var allIems = data.items;
        //                       for(var element in allIems) {
        //                         document.getElementById('datafromapi').innerHTML =
        //                         ( " , published time :" + element.published
        //                             + " , last updated time :" + element.updated 
        //                             +", complete url : " + element.url

        //                         );
        //                         var obj = allIems[element].object.attachments;

        //                         document.getElementById('datafromapi').innerHTML +=
        //                         (
        //                             + "<br>attachments of post :<br>" +
        //                                 " type : " +
        //                                 obj[0].objectType +
        //                                 "<br> ,displayName of content : " +
        //                                 obj[0].displayName +
        //                                 "<br> ,content URL  : " +
        //                                 obj[0].url +
        //                                 " <br>,content data :" +
        //                                 obj[0].image.url +
        //                                 " <br>,content type : " +
        //                                 obj[0].image.type +
        //                                 "<br> ,content height : " +
        //                                 obj[0].image.height +
        //                                 "<br> ,content width : " +
        //                                 obj[0].image.width +
        //                                 "<br><br><br>");


        //                     }  */
        //            });
        //            /*  console.log('Activity content: ' + resp2.object.content);

        //              console.log('+1s: ' + resp2.object.plusoners.totalItems);
        //              console.log('Number of comments: ' + resp2.object.replies.totalItems);
        //              */
        //        });

        //    }
    });

}

function logout() {
    gapi.auth.signOut();
    location.reload();
}

function login() {
    
    var myParams = {
        'clientid': '417470137876-3bss1abem5h0ghr9oois2g691uqto00o.apps.googleusercontent.com',
        'cookiepolicy': 'single_host_origin',
        'callback': 'loginCallback',
        'approvalprompt': 'force',
        'scope': 'https://www.googleapis.com/auth/plus.login https://www.googleapis.com/auth/plus.profile.emails.read ' +
            'https://www.googleapis.com/auth/plus.me'
    };
    
    gapi.auth.signIn(myParams);
    
}

function loginCallback(result) {
    alert("call back");
    if (result['status']['signed_in']) {
        var request = gapi.client.plus.people.get(
            {
                'userId': 'me'
            });
        request.execute(function (resp) {
            var email = '';
            if (resp['emails']) {
                for (i = 0; i < resp['emails'].length; i++) {
                    if (resp['emails'][i]['type'] == 'account') {
                        email = resp['emails'][i]['value'];
                    }
                }
            }

            var str = "Name:" + resp['displayName'] + "<br>";
            str += "Image:" + resp['image']['url'] + "<br>";
            str += "<img src='" + resp['image']['url'] + "' /><br>";
            str += "id: " + resp['id'] + "<br>";
            str += "URL:" + resp['url'] + "<br>";
            str += "Email:" + email + "<br>";
            GuserID = resp['id'];


        });

    }

}

function onLoadCallback() {
    gapi.client.setApiKey('AIzaSyD_oNG0Jw7zI0q8TJNRd0WphZbabUTXOPc');
    gapi.client.load('plus', 'v1', function () { });
}



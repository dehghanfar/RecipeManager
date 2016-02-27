function AddRecipe() {
   
    var url ='/Home/AddRecipeOrders';
   
    $.post(url, {

        itemUnit: toDecimal($("#Unit").val()),

        itemId: $("#IngId").val(),

        itemName: $("#IngId option:selected").text()

    })
        .done(function (data) {

           
            if (data.message === "Success") {
             
                $('#DivGrid').load( '/Home/_RecipeOrdersList');

            }
            if (data.message === "ZeroEntry") {

                $("#MessageDiv").replaceWith("<div id=MessageDiv class='alert alert-warning' >Please check the unit value is valid!</div>");

            }
            if (data.message === "Failed") {

                $("#MessageDiv").replaceWith("<div id=MessageDiv class='alert alert-warning' >Something went wrong in adding a new item!</div>");
            
            }

        });

};


function RemoveRecipe(itemId, unitId) {

    if (confirm("Are you sure?")) {
      
        var url = '/Home/RemoveRecipeOrders';
     
        $.post(url, {
            itemUnit: unitId,

            itemId: itemId

            })
            .done(function (data) {
                if (data.message === "Success") {
                    $('#DivGrid').load('/Home/_RecipeOrdersList');

                }
                if (data.message === "Failed") {
                    $('#DivGrid').load('/Home/_RecipeOrdersList');
                    $("#MessageDiv").replaceWith("<div id=MessageDiv class='alert alert-warning' >Something went wrong in removing an item!</div>");

                }


            });
    }

};



function Calculate() {
   
    var url = '/Home/CalculateFinalRecipt';
    
    setTimeout("jQuery('.LoadingSpin').hide();", 15000);
    $.get(url)
        .done(function () {
                $('#DivGridRecipt').load('/Home/CalculateFinalRecipt');

        });

};



function toDecimal(x) {


    if (x.indexOf("/") > -1) {

        var parts = x.split(" ");

        if (parts.length > 1) {
            var decParts = parts[1].split("/");
            return parseInt(parts[0], 10) +
            (parseInt(decParts[0], 10) / parseInt(decParts[1], 10));
        } else {
            var decParts2 = parts[0].split("/");
            return parseInt(decParts2[0], 10) / parseInt(decParts2[1], 10);
        }
    } else {
        return x;
    }
}
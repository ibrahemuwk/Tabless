$(document).ready(function () {
    //the show more span(first of type)
    $(".item span").click(function () {
        $(this).find('i').toggleClass('downicon');
        $(this).find('i').toggleClass('upicon');
        $(this).parent().find("div.content").toggleClass('show');
        /* var x=$(this).parent().siblings().find('div:last-child').addClass('show');
         if(!$(this).parent().siblings().find('div:last-child').find('div.show')){
            $(this).addClass('show'); 
            $(".container .item span").find('i').removeClass('downicon');
            $(".container .item span").find('i').addClass('upicon');
         }*/



    });
});
    
    
    
 
  
    
    

  

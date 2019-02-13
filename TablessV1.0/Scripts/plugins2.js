$(document).ready(function(){
   
 
//the header arrow
$(".arrow i").click(function(){
    $('html,body').animate({
        scrollTop:$(".feat").offset().top   
    },1000);
});    
    
    
    
 //adjust header background to window height 
 $(".header").height($(window).height());      
    
 //trigger the up div
  $(window).scroll(function(){
      var sc=$(this).scrollTop();

      if(sc >500){
          $(".up").fadeIn(1000)
      }
      else{
        $(".up").fadeOut(1000)  
      }
      
  }); 
    
 //make the scrolling equal to 0    
  $(".up").click(function(){   
  //  $(window).scrollTop(0);
      
        $('html,body').animate({
        scrollTop:0   
    },1000);

    });
    
    
 //trigger the nice scroll 
    $("html").niceScroll({
        cursorcolor:'#f7600e',
        cursorwidth:'8px',
        cursorborder:'1px solid #f7600e'
    });    
     
});
























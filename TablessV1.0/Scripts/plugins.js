$(function(){
var mail=$(".mail"),
    subject=$(".subject"),
    message=$(".message"),
    sub=$(".upper");
    
    
    'use strict';
    $(".mail").blur(function(){
      
        if($(this).val()==""){
            $(this).css('border','1px solid #f00');
            $(this).attr("placeholder","Enter Your Mail");
        }else{
            $(this).css('border','1px solid #0f0');
            $(this).attr("placeholder","Your Mail");
        }
        
    });
    

     $(".subject").blur(function(){
      
        if($(this).val()==""){
            $(this).css('border','1px solid #f00');
            $(this).attr("placeholder","Enter The Subject");
        }else{
            $(this).next().fadeOut(300);
            $(this).attr("placeholder","Subject");
        }
        
    });
    
    
     $(".message").blur(function(){
      
        if($(this).val()==""){
            $(this).css('border','1px solid #f00');
            $(this).attr("placeholder","Enter Your Message");
        }else{
            $(this).css('border','1px solid #0f0');
            $(this).attr("placeholder","Message");
        }
        
    });
        
    sub.click(function(event){
        if(mail.val()==""){
            event.preventDefault(); 
            mail.css('border','1px solid #f00');
            mail.attr("placeholder","Enter Your Mail");
            
        }
        
        if(subject.val()==""){
            event.preventDefault(); 
            subject.css('border','1px solid #f00'); 
            subject.attr("placeholder","Enter The Subject");
            
        }
        
        if(message.val()==""){
            event.preventDefault(); 
            message.css('border','1px solid #f00');   
            message.attr("placeholder","Enter Your Message");
            
        }
        
      
        
    });
    
    
});
    
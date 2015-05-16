/*global jQuery:false */
jQuery(document).ready(function($) {
"use strict";

	//Contact
	$('form.contactForm').submit(function(){

		var f = $(this).find('.form-group'), 
		ferror = false, 
		emailExp = /^[^\s()<>@,;:\/]+@\w[\w\.-]+\.[a-z]{2,}$/i;

		f.children('input').each(function(){ // run all inputs

			var i = $(this); // current input
			var rule = i.attr('data-rule');

			if( rule !== undefined ){
			var ierror=false; // error flag for current input
			var pos = rule.indexOf( ':', 0 );
			if( pos >= 0 ){
				var exp = rule.substr( pos+1, rule.length );
				rule = rule.substr(0, pos);
			}else{
				rule = rule.substr( pos+1, rule.length );
			}
			
			switch( rule ){
				case 'required':
				if( i.val()==='' ){ ferror=ierror=true; }
				break;
				
				case 'maxlen':
				if( i.val().length<parseInt(exp) ){ ferror=ierror=true; }
				break;

				case 'email':
				if( !emailExp.test(i.val()) ){ ferror=ierror=true; }
				break;

				case 'checked':
				if( !i.attr('checked') ){ ferror=ierror=true; }
				break;
				
				case 'regexp':
				exp = new RegExp(exp);
				if( !exp.test(i.val()) ){ ferror=ierror=true; }
				break;
			}
				i.next('.validation').html( ( ierror ? (i.attr('data-msg') !== undefined ? i.attr('data-msg') : 'wrong Input') : '' ) ).show('blind');
			}
		});
		f.children('textarea').each(function(){ // run all inputs

			var i = $(this); // current input
			var rule = i.attr('data-rule');

			if( rule !== undefined ){
			var ierror=false; // error flag for current input
			var pos = rule.indexOf( ':', 0 );
			if( pos >= 0 ){
				var exp = rule.substr( pos+1, rule.length );
				rule = rule.substr(0, pos);
			}else{
				rule = rule.substr( pos+1, rule.length );
			}
			
			switch( rule ){
				case 'required':
				if( i.val()==='' ){ ferror=ierror=true; }
				break;
				
				case 'maxlen':
				if( i.val().length<parseInt(exp) ){ ferror=ierror=true; }
				break;
			}
				i.next('.validation').html( ( ierror ? (i.attr('data-msg') != undefined ? i.attr('data-msg') : 'wrong Input') : '' ) ).show('blind');
			}
		});
		if( ferror ) return false; 
		var name = document.getElementById("name").value;
		var fromEmail = document.getElementById("email").value;
		var subject = document.getElementById("subject").value;
		var message = document.getElementById("userMessage").value;
		var data = "{'name': '" + name + "', 'fromEmail': '" +
                   fromEmail + "', 'subject': '" + subject + "', 'message': '" + message + "'}";
				$.ajax({
				type: "POST",
				url: "SendEmail.aspx",
				data: data,
				contentType: "application/x-www-form-urlencoded; charset=UTF-8",
				dataType: "json",
				success: function(msg){
			$("#sendmessage").addClass("show");
			$("#errormessage").ajaxComplete(function(event, request, settings){
		
			if(msg == 'OK')
			{
				$("#sendmessage").addClass("show");				
			}
			else
			{
				$("#sendmessage").removeClass("show");
				result = msg;
			}
		
			$(this).html(result);});}});
				return false;
	});

	$('form.registerForm').submit(function () {

	    var f = $(this).find('.form-group'),
		ferror = false,
		emailExp = /^[^\s()<>@,;:\/]+@\w[\w\.-]+\.[a-z]{2,}$/i;

	    f.children('input').each(function () { // run all inputs

	        var i = $(this); // current input
	        var rule = i.attr('data-rule');

	        if (rule !== undefined) {
	            var ierror = false; // error flag for current input
	            var errorCode = 0;
	            var pos = rule.indexOf(':', 0);
	            if (pos >= 0) {
	                var exp = rule.substr(pos + 1, rule.length);
	                rule = rule.substr(0, pos);
	            } else {
	                rule = rule.substr(pos + 1, rule.length);
	            }

	            switch (rule) {
	                case 'required':
	                    if (i.val() === '') { ferror = ierror = true; errorCode = 1;}
	                    break;

	                case 'maxlen':
	                    if (i.val().length < parseInt(exp)) { ferror = ierror = true; errorCode = 2; }
	                    break;

	                case 'email':
	                    if (!emailExp.test(i.val())) { ferror = ierror = true; errorCode = 3; }
	                    break;

	                case 'checked':
	                    if (!i.attr('checked')) { ferror = ierror = true; errorCode = 4; }
	                    break;

	                case 'regexp':
	                    exp = new RegExp(exp);
	                    if (!exp.test(i.val())) { ferror = ierror = true; errorCode = 5; }
	                    break;
	            }
	            var message = "";
	            if (ierror) {
	                switch(errorCode){
	                    case "1":
	                        message = (i.attr('data-val-required') !== undefined ? i.attr('data-val') : 'wrong Input');
	                        break;
	                    case "2":
	                        message = (i.attr('data-val-length') !== undefined ? i.attr('data-val') : 'wrong Input');
	                        break;
	                    case "3":
	                        message = (i.attr('data-val-email') !== undefined ? i.attr('data-val') : 'wrong Input');
	                        break;
	                    default:
	                        message = (i.attr('data-val') !== undefined ? i.attr('data-val') : 'wrong Input');
	                        break;
	                }
	            }
	            
	            i.next('.validation').html(message).show('blind');
	        }
	    });
	    if (ferror) return false;
	    var username = document.getElementById("Userame").value;
	    var email = document.getElementById("Email").value;
	    var password = document.getElementById("Password").value;
	    var name = document.getElementById("FirstName").value;
	    var surname = document.getElementById("LastName").value;
	    var data = "{'Username': '" + username + "', 'Email': '" +
                   email + "', 'Password': '" + password + "', 'FirstName': '" +
                   name + "', 'LastName': '" + surname + "'}";
	    $.ajax({
	        type: "POST",
	        url: "Register/Account",
	        data: data,
	        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
	        dataType: "json",
	        success: function(msg){
	            $("#sendmessage").addClass("show");
	            $("#errormessage").ajaxComplete(function(event, request, settings){
		
	                if(msg == 'OK')
	                {
	                    $("#sendmessage").addClass("show");				
	                }
	                else
	                {
	                    $("#sendmessage").removeClass("show");
	                    result = msg;
	                }
		
	                $(this).html(result);});}});
	    return false;
	});

});
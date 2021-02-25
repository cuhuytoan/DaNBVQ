
$(document).ready(function(){
	// fadeIn() for smoth loading
	$(".menu-container").fadeIn();
	
	
	$(".menu-container .manu ul li:has(ul)").children("ul").not("ul li ul li ul").addClass("have_dropdown");
	
	//Checks if drodown menu's li elements have anothere level (ul), if not the dropdown is shown as regular dropdown, not a mega menu
	$(".menu-container .manu ul li ul:has(li ul li)").removeClass("have_dropdown").addClass("have_dropdown_n_style");
	
	 //Checks if li has sub (ul) and adds class for toggle icon 
	$(".menu-container .manu ul li:has('ul')").children("div").not(".manu ul li ul li a").append("<span class='navbar-menu navbar-toggler-icon'></span>");
	
	
	if ($(window).width() < 992) {
		$(".menu-container .manu > nav > ul > li > div > .navbar-toggler-icon").click(function(e) {
			if ($(this).hasClass('active')) {
				$(this).removeClass("active");
			} else {
				$(this).addClass("active");
			}
			if ($(this).parent().parent().find('ul').hasClass('have_dropdown')) {
				e.preventDefault();
				$(this).parent().parent('li').children(".have_dropdown").not(".have_dropdown li a").slideToggle(100);
			} 
		});
		
     } else {
		$(".menu-container .manu ul li").hover(function(){
			$(this).children(".have_dropdown").not(".have_dropdown li a").slideDown(100);
		},function(){
			$(this).children(".have_dropdown").not(".have_dropdown li a").css("display", "none");
		});

		$(".menu-container .manu ul li").hover(function(){
			$(this).children(".have_dropdown_n_style").not(".have_dropdown_n_style li a").slideDown(100);
		},function(){
			$(this).children(".have_dropdown_n_style").not(".have_dropdown_n_style li a").css("display", "none");
		}); 
	 }
});
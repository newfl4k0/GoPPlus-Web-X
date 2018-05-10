//(function( factory ) {
//	if ( typeof define === "function" && define.amd ) {
//		define( ["jquery", "../jquery.validate"], factory );
//	} else if (typeof module === "object" && module.exports) {
//		module.exports = factory( require( "jquery" ) );
//	} else {
//		factory( jQuery );
//	}
//}
(function ($) {

/*
 * Translated default messages for the jQuery validation plugin.
 * Locale: ES (Spanish; Español)
 */
jQuery.extend(jQuery.validator.messages, {
	required: "Este campo es obligatorio.",
	remote: "Por favor, rellena este campo.",
	email: "Por favor, escribe una dirección de correo válida.",
	url: "Por favor, escribe una URL válida.",
	date: "Por favor, escribe una fecha válida.",
	dateISO: "Por favor, escribe una fecha (ISO) válida.",
	number: "Por favor, escribe un número válido.",
	digits: "Por favor, escribe sólo dígitos.",
	creditcard: "Por favor, escribe un número de tarjeta válido.",
	equalTo: "Por favor, escribe el mismo valor de nuevo.",
	extension: "Por favor, escribe un valor con una extensión aceptada.",
	maxlength: jQuery.validator.format("Por favor, no escribas más de {0} caracteres."),
	minlength: jQuery.validator.format("Por favor, no escribas menos de {0} caracteres."),
	rangelength: jQuery.validator.format("Por favor, escribe un valor entre {0} y {1} caracteres."),
	range: jQuery.validator.format("Por favor, escribe un valor entre {0} y {1}."),
	max: jQuery.validator.format("Por favor, escribe un valor menor o igual a {0}."),
	min: jQuery.validator.format("Por favor, escribe un valor mayor o igual a {0}."),
	nifES: "Por favor, escribe un NIF válido.",
	nieES: "Por favor, escribe un NIE válido.",
	cifES: "Por favor, escribe un CIF válido."
} );

}
)//);
var map = null;

function GetMap() {
    map = new VEMap('myMap');
    map.LoadMap();
    var options = new VERouteOptions();
    options.RouteCallback = onGotRoute;
    map.GetDirections(["Mahan Dr, Tallahassee, FL 32317", "142 Collegiate Way, Tallahassee, FL 32306"], options);
}

function onGotRoute(route) {
	// Unroll route           
	var legs = route.RouteLegs;
	var turns = "Total distance: " + route.Distance.toFixed(1) + " mi\n";
	var numTurns = 0;
	var leg = null;

	// Get intermediate legs          
	for (var i = 0; i < legs.length; i++) {
		// Get this leg so we don't have to derefernce multiple times
		leg = legs[i];  // Leg is a VERouteLeg object

		// Unroll each intermediate leg  
		var turn = null;  // The itinerary leg                                 
		for (var j = 0; j < leg.Itinerary.Items.length; j++) {
			turn = leg.Itinerary.Items[j];  // turn is a VERouteItineraryItem object
			numTurns++;
			turns += numTurns + ".\t" + turn.Text + " (" + turn.Distance.toFixed(1) + " mi)\n";
		}
	}
}

$(document).ready(function() { GetMap(); });
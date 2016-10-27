function leftPad(input, length) {
	str = input.toString();
    while (str.length < length) {
        str = '0' + str;
    }
    return str;
}

function createGripColors(hex) {
	if (hex == '#000000') {
		hex = '#4B4B4B';
	}
	var high = hexToRgb(hex);
	var low = [], mid = [];

	for (var i = 0; i < 3; i++) {
		if (high[i] < 75) {
			low[i] = 0;
		} else {
			low[i] = high[i] - 75;
		}
		if (high[i] < 35) {
			mid[i] = 0;
		} else {
			mid[i] = high[i] - 35;
		}
	}
	return [rgbToHex(...low), rgbToHex(...mid), rgbToHex(...high)];
}

/* http://stackoverflow.com/a/5624139 */
function rgbToHex(r, g, b) {
    return "#" + componentToHex(r) + componentToHex(g) + componentToHex(b);
}

/* http://stackoverflow.com/a/5624139 */
function componentToHex(c) {
    var hex = c.toString(16);
    return hex.length == 1 ? '0' + hex : hex;
}

/* http://stackoverflow.com/a/5624139 */
function hexToRgb(hex) {
    var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
    return result ? [
        parseInt(result[1], 16),
        parseInt(result[2], 16),
        parseInt(result[3], 16)
    ] : null;
}

$(document).ready(function() {
	function constructRoute(route) {
		/*
		var gradeColors = ['green', 'blue', 'red', 'black', 'white'];

		var gripColorRGB = '#' + (route['colorOfHolds'] >>> 8).toString(16).lpad('0', 6);
		var gripColors = createGripColors(gripColorRGB);

		var date = new Date(route['date']);
		var day = date.getDate().toString().lpad('0', 2);
		var month = (date.getMonth() + 1).toString();
		var year = date.getFullYear().toString().substring(2);
		var dateString = day + '/' + month + '/' + year;

		var routehtml = $(`
			<div class="route">
				<div class="route-id">
					<div class="grade grade-${gradeColors[route['grade']]} fl">
						<span class="number">${route['name']}</span>
					</div>
					<div class="route-section fl">
						<span>Section: </span>
						<span class="route-section-letter">${route['sectionID']}</span>
					</div>
				</div>
				<div class="date fr">${dateString}</div>
				<div class="grip fl">
					<svg viewbox="0 0 132 108" width="132" height="110" xmlns="http://www.w3.org/2000/svg">
					    <g id="svg_1">
					        <path id="svg_2" d="m77.3,6.65c-10.7,5.3 -33.1,15.5 -56.8,20.5c-7.6,1.6 -13.5,7.5 -14.8,15.2c-1.1,6.2 -1.7,14 -0.6,22c1.2,8.9 8.6,15.6 17.6,16.1c13.7,0.8 36.5,4.8 55.2,21.6c5.7,5.1 14.1,6.1 20.9,2.6c8.3,-4.2 19,-11.2 27.7,-22.4c5,-6.5 5.5,-15.5 0.8,-22.2c-4.4,-6.3 -9.3,-15.9 -10.3,-28.7c-0.4,-4.5 -2.2,-8.7 -5.4,-11.8c-4.1,-4.1 -10.3,-9.4 -17.6,-13c-5.3,-2.5 -11.5,-2.5 -16.7,0.1z" fill="${gripColors[1]}"/>
					        <path id="svg_3" d="m98.4,80.75c27.7,-10.8 1.9,-42.2 13.1,-61c-4.1,-4.1 -10.3,-9.4 -17.6,-13c-5.2,-2.6 -11.4,-2.6 -16.6,0c-10.7,5.3 -33.1,15.5 -56.8,20.5c-1.8,0.4 -3.5,1 -5,1.8l0,0c0,0 22.8,-4.7 37.6,20.5c14.6,25.1 17.6,41.9 45.3,31.2z" fill="${gripColors[2]}"/>
					        <g id="svg_4">
					            <circle id="svg_5" r="11.7" cy="57.25" cx="90.6" fill="#E6E7E8"/>
					            <circle id="svg_6" r="7.8" cy="57.25" cx="90.6" fill="#414042"/>
					        </g>
					        <path id="svg_7" d="m126.5,82.35c2.5,-3.2 3.8,-7 4,-10.8c-8,7.9 -22,20.7 -32.1,25.2c-14.9,6.7 -26.6,-12.1 -56.1,-20.8c-14.2,-4.2 -24.9,-3.2 -32.1,-1.1c3.2,3.3 7.6,5.4 12.5,5.6c13.7,0.8 36.5,4.8 55.2,21.6c5.7,5.1 14.1,6.1 20.9,2.6c8.3,-4.1 19,-11.1 27.7,-22.3z" fill="${gripColors[0]}"/>
					    </g>
					</svg>
				</div>
				<div class="author fr">
					<span>Made by:</span>
					<span class="author-name">${route['author']}</span>
				</div>
			</div>`);

		routehtml.appendTo($('.list'));
		*/
		var nextRoute = new Route(data['grade'], data['name'], data['sectionID'], data['author'], data['colorOfHolds'], data['date']);
		nextRoute
	}

	/* http://stackoverflow.com/a/10073737 */
	String.prototype.lpad = function(padString, length) {
	    var str = this;
	    while (str.length < length)
	        str = padString + str;
	    return str;
	}

	function createGripColors(hex) {
		if (hex == '#000000') {
			hex = '#4B4B4B';
		}
		var high = hexToRgb(hex);
		var low = [], mid = [];

		for (var i = 0; i < 3; i++) {
			if (high[i] < 75) {
				low[i] = 0;
			} else {
				low[i] = high[i] - 75;
			}
			if (high[i] < 35) {
				mid[i] = 0;
			} else {
				mid[i] = high[i] - 35;
			}
		}

		return [rgbToHex(...low), rgbToHex(...mid), rgbToHex(...high)];
	}

	$.getJSON('api/route', function(data) {
		data.forEach(constructRoute);
	});
});
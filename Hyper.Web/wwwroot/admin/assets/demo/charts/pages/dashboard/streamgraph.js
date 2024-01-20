/* ------------------------------------------------------------------------------
 *
 *  # D3.js - streamgraph
 *
 *  Demo of streamgraph chart setup with tooltip and .csv data source
 *
 * ---------------------------------------------------------------------------- */


// Setup module
// ------------------------------

var D3Streamgraph = function() {


    //
    // Setup module components
    //

    // Chart
    var _streamgraph = function() {
        if (typeof d3 == 'undefined') {
            console.warn('Warning - d3.min.js is not loaded.');
            return;
        }

        // Main variables
        var element = document.getElementById('traffic-sources'),
            height = 340;


        // Initialize chart only if element exsists in the DOM
        if(element) {

            // Basic setup
            // ------------------------------

            // Define main variables
            var d3Container = d3.select(element),
                margin = {top: 5, right: 50, bottom: 40, left: 50},
                width = d3Container.node().getBoundingClientRect().width - margin.left - margin.right,
                height = height - margin.top - margin.bottom,
                tooltipOffset = 30;

            // Tooltip
            var tooltip = d3Container
                .append("div")
                .attr("class", "d3-tip e")
                .style("display", "none")

            // Format date
            var format = d3.time.format("%m/%d/%y %H:%M");
            var formatDate = d3.time.format("%H:%M");


            // Use CSS vars for easy color switching
            var colors = d3Container
                .append('style')
                .attr('type', 'text/css')
                .html(`
                    .streamgraph-layers-group {
                        --sg-color-1: #03A9F4;
                        --sg-color-2: #29B6F6;
                        --sg-color-3: #4FC3F7;
                        --sg-color-4: #81D4FA;
                        --sg-color-5: #B3E5FC;
                        --sg-color-6: #E1F5FE;
                    }

                    [data-color-theme="dark"] .streamgraph-layers-group {
                        --sg-color-1: #225ea8;
                        --sg-color-2: #1e90c0;
                        --sg-color-3: #40b6c4;
                        --sg-color-4: #7fcdbb;
                        --sg-color-5: #c7e8b4;
                        --sg-color-6: #edf8b1;
                    }
                `);

            // Colors
            var colorrange = ['var(--sg-color-1)', 'var(--sg-color-2)', 'var(--sg-color-3)', 'var(--sg-color-4)', 'var(--sg-color-5)', 'var(--sg-color-6)'];



            // Construct scales
            // ------------------------------

            // Horizontal
            var x = d3.time.scale().range([0, width]);

            // Vertical
            var y = d3.scale.linear().range([height, 0]);

            // Colors
            var z = d3.scale.ordinal().range(colorrange);



            // Create axes
            // ------------------------------

            // Horizontal
            var xAxis = d3.svg.axis()
                .scale(x)
                .orient("bottom")
                .ticks(d3.time.hours, 4)
                .innerTickSize(4)
                .tickPadding(8)
                .tickFormat(d3.time.format("%H:%M")); // Display hours and minutes in 24h format

            // Left vertical
            var yAxis = d3.svg.axis()
                .scale(y)
                .ticks(6)
                .innerTickSize(4)
                .outerTickSize(0)
                .tickPadding(8)
                .tickFormat(function (d) { return (d/1000) + "k"; });

            // Right vertical
            var yAxis2 = yAxis;

            // Dash lines
            var gridAxis = d3.svg.axis()
                .scale(y)
                .orient("left")
                .ticks(6)
                .tickPadding(8)
                .tickFormat("")
                .tickSize(-width, 0, 0);



            // Create chart
            // ------------------------------

            // Container
            var container = d3Container.append("svg")

            // SVG element
            var svg = container
                .attr('width', width + margin.left + margin.right)
                .attr("height", height + margin.top + margin.bottom)
                    .append("g")
                    .attr("transform", "translate(" + margin.left + "," + margin.top + ")");



            // Construct chart layout
            // ------------------------------

            // Stack
            var stack = d3.layout.stack()
                .offset("silhouette")
                .values(function(d) { return d.values; })
                .x(function(d) { return d.date; })
                .y(function(d) { return d.value; });

            // Nest
            var nest = d3.nest()
                .key(function(d) { return d.key; });

            // Area
            var area = d3.svg.area()
                .interpolate("cardinal")
                .x(function(d) { return x(d.date); })
                .y0(function(d) { return y(d.y0); })
                .y1(function(d) { return y(d.y0 + d.y); });



            // Load data
            // ------------------------------


            var csvData = [

                "Youtube videos,100,01/06/15 00:00",
                "Youtube videos,150,01/06/15 01:00",
                "Youtube videos,350,01/06/15 02:00",
                "Youtube videos,380,01/06/15 03:00",
                "Youtube videos,220,01/06/15 04:00",
                "Youtube videos,160,01/06/15 05:00",
                "Youtube videos,170,01/06/15 06:00",
                "Youtube videos,120,01/06/15 07:00",
                "Youtube videos,170,01/06/15 08:00",
                "Youtube videos,330,01/06/15 09:00",
                "Youtube videos,400,01/06/15 10:00",
                "Youtube videos,320,01/06/15 11:00",
                "Youtube videos,260,01/06/15 12:00",
                "Youtube videos,350,01/06/15 13:00",
                "Youtube videos,400,01/06/15 14:00",
                "Youtube videos,320,01/06/15 15:00",
                "Youtube videos,260,01/06/15 16:00",
                "Youtube videos,220,01/06/15 17:00",
                "Youtube videos,160,01/06/15 18:00",
                "Youtube videos,220,01/06/15 19:00",
                "Youtube videos,250,01/06/15 20:00",
                "Youtube videos,220,01/06/15 21:00",
                "Youtube videos,150,01/06/15 22:00",
                "Youtube videos,390,01/06/15 23:00",
                "Youtube videos,440,01/06/15 24:00",
                "Facebook ads,350,01/06/15 00:00",
                "Facebook ads,360,01/06/15 01:00",
                "Facebook ads,370,01/06/15 02:00",
                "Facebook ads,220,01/06/15 03:00",
                "Facebook ads,240,01/06/15 04:00",
                "Facebook ads,260,01/06/15 05:00",
                "Facebook ads,340,01/06/15 06:00",
                "Facebook ads,210,01/06/15 07:00",
                "Facebook ads,180,01/06/15 08:00",
                "Facebook ads,450,01/06/15 09:00",
                "Facebook ads,320,01/06/15 10:00",
                "Facebook ads,350,01/06/15 11:00",
                "Facebook ads,300,01/06/15 12:00",
                "Facebook ads,280,01/06/15 13:00",
                "Facebook ads,270,01/06/15 14:00",
                "Facebook ads,260,01/06/15 15:00",
                "Facebook ads,150,01/06/15 16:00",
                "Facebook ads,300,01/06/15 17:00",
                "Facebook ads,350,01/06/15 18:00",
                "Facebook ads,220,01/06/15 19:00",
                "Facebook ads,220,01/06/15 20:00",
                "Facebook ads,350,01/06/15 21:00",
                "Facebook ads,300,01/06/15 22:00",
                "Facebook ads,220,01/06/15 23:00",
                "Facebook ads,320,01/06/15 24:00",
                "Twitter ads,210,01/06/15 00:00",
                "Twitter ads,250,01/06/15 01:00",
                "Twitter ads,270,01/06/15 02:00",
                "Twitter ads,230,01/06/15 03:00",
                "Twitter ads,240,01/06/15 04:00",
                "Twitter ads,110,01/06/15 05:00",
                "Twitter ads,200,01/06/15 06:00",
                "Twitter ads,290,01/06/15 07:00",
                "Twitter ads,300,01/06/15 08:00",
                "Twitter ads,360,01/06/15 09:00",
                "Twitter ads,330,01/06/15 10:00",
                "Twitter ads,430,01/06/15 11:00",
                "Twitter ads,400,01/06/15 12:00",
                "Twitter ads,340,01/06/15 13:00",
                "Twitter ads,280,01/06/15 14:00",
                "Twitter ads,260,01/06/15 15:00",
                "Twitter ads,270,01/06/15 16:00",
                "Twitter ads,310,01/06/15 17:00",
                "Twitter ads,260,01/06/15 18:00",
                "Twitter ads,370,01/06/15 19:00",
                "Twitter ads,210,01/06/15 20:00",
                "Twitter ads,260,01/06/15 21:00",
                "Twitter ads,370,01/06/15 22:00",
                "Twitter ads,310,01/06/15 23:00",
                "Twitter ads,350,01/06/15 24:00",
                "Google search,100,01/06/15 00:00",
                "Google search,150,01/06/15 01:00",
                "Google search,350,01/06/15 02:00",
                "Google search,380,01/06/15 03:00",
                "Google search,220,01/06/15 04:00",
                "Google search,160,01/06/15 05:00",
                "Google search,170,01/06/15 06:00",
                "Google search,120,01/06/15 07:00",
                "Google search,170,01/06/15 08:00",
                "Google search,330,01/06/15 09:00",
                "Google search,400,01/06/15 10:00",
                "Google search,320,01/06/15 11:00",
                "Google search,260,01/06/15 12:00",
                "Google search,350,01/06/15 13:00",
                "Google search,400,01/06/15 14:00",
                "Google search,320,01/06/15 15:00",
                "Google search,260,01/06/15 16:00",
                "Google search,220,01/06/15 17:00",
                "Google search,160,01/06/15 18:00",
                "Google search,220,01/06/15 19:00",
                "Google search,100,01/06/15 20:00",
                "Google search,160,01/06/15 21:00",
                "Google search,220,01/06/15 22:00",
                "Google search,400,01/06/15 23:00",
                "Google search,300,01/06/15 24:00",
                "Dribbble ads,100,01/06/15 00:00",
                "Dribbble ads,150,01/06/15 01:00",
                "Dribbble ads,350,01/06/15 02:00",
                "Dribbble ads,380,01/06/15 03:00",
                "Dribbble ads,220,01/06/15 04:00",
                "Dribbble ads,160,01/06/15 05:00",
                "Dribbble ads,170,01/06/15 06:00",
                "Dribbble ads,120,01/06/15 07:00",
                "Dribbble ads,170,01/06/15 08:00",
                "Dribbble ads,330,01/06/15 09:00",
                "Dribbble ads,400,01/06/15 10:00",
                "Dribbble ads,320,01/06/15 11:00",
                "Dribbble ads,260,01/06/15 12:00",
                "Dribbble ads,350,01/06/15 13:00",
                "Dribbble ads,400,01/06/15 14:00",
                "Dribbble ads,320,01/06/15 15:00",
                "Dribbble ads,260,01/06/15 16:00",
                "Dribbble ads,220,01/06/15 17:00",
                "Dribbble ads,160,01/06/15 18:00",
                "Dribbble ads,220,01/06/15 19:00",
                "Dribbble ads,100,01/06/15 20:00",
                "Dribbble ads,160,01/06/15 21:00",
                "Dribbble ads,220,01/06/15 22:00",
                "Dribbble ads,400,01/06/15 23:00",
                "Dribbble ads,300,01/06/15 24:00",
                "Amazon ads,100,01/06/15 00:00",
                "Amazon ads,150,01/06/15 01:00",
                "Amazon ads,350,01/06/15 02:00",
                "Amazon ads,380,01/06/15 03:00",
                "Amazon ads,220,01/06/15 04:00",
                "Amazon ads,160,01/06/15 05:00",
                "Amazon ads,170,01/06/15 06:00",
                "Amazon ads,120,01/06/15 07:00",
                "Amazon ads,170,01/06/15 08:00",
                "Amazon ads,330,01/06/15 09:00",
                "Amazon ads,400,01/06/15 10:00",
                "Amazon ads,320,01/06/15 11:00",
                "Amazon ads,260,01/06/15 12:00",
                "Amazon ads,350,01/06/15 13:00",
                "Amazon ads,400,01/06/15 14:00",
                "Amazon ads,320,01/06/15 15:00",
                "Amazon ads,260,01/06/15 16:00",
                "Amazon ads,220,01/06/15 17:00",
                "Amazon ads,160,01/06/15 18:00",
                "Amazon ads,220,01/06/15 19:00",
                "Amazon ads,100,01/06/15 20:00",
                "Amazon ads,160,01/06/15 21:00",
                "Amazon ads,220,01/06/15 22:00",
                "Amazon ads,300,01/06/15 23:00",
                "Amazon ads,350,01/06/15 24:00",
            ]

            d3.csv("admin/assets/demo/data/dashboard/traffic_sources.csv", function (error, data) {

                // Pull out values
                data.forEach(function (d) {
                    d.date = format.parse(d.date);
                    d.value = +d.value;
                });

                // Stack and nest layers
                var layers = stack(nest.entries(data));



                // Set input domains
                // ------------------------------

                // Horizontal
                x.domain(d3.extent(data, function(d, i) { return d.date; }));

                // Vertical
                y.domain([0, d3.max(data, function(d) { return d.y0 + d.y; })]);



                // Add grid
                // ------------------------------

                // Horizontal grid. Must be before the group
                svg.append("g")
                    .attr("class", "d3-grid-dashed")
                    .call(gridAxis);



                //
                // Append chart elements
                //

                // Stream layers
                // ------------------------------

                // Create group
                var group = svg.append('g')
                    .attr('class', 'streamgraph-layers-group');

                // And append paths to this group
                var layer = group.selectAll(".streamgraph-layer")
                    .data(layers)
                    .enter()
                        .append("path")
                        .attr("class", "streamgraph-layer d3-slice-border")
                        .attr("d", function(d) { return area(d.values); })                    
                        .style('stroke-width', 1)
                        .style('box-shadow', '0 4px 8px rgba(0,0,0,0.5)')
                        .style("fill", function(d, i) { return z(i); });

                // Add transition
                var layerTransition = layer
                    .style('opacity', 0)
                    .transition()
                        .duration(750)
                        .delay(function(d, i) { return i * 50; })
                        .style('opacity', 1)



                // Append axes
                // ------------------------------

                //
                // Left vertical
                //

                svg.append("g")
                    .attr("class", "d3-axis d3-axis-left")
                    .call(yAxis.orient("left"));

                // Hide first tick
                d3.select(svg.selectAll('.d3-axis-left .tick text')[0][0])
                    .style("visibility", "hidden");


                //
                // Right vertical
                //

                svg.append("g")
                    .attr("class", "d3-axis d3-axis-right")
                    .attr("transform", "translate(" + width + ", 0)")
                    .call(yAxis2.orient("right"));

                // Hide first tick
                d3.select(svg.selectAll('.d3-axis-right .tick text')[0][0])
                    .style("visibility", "hidden");


                //
                // Horizontal
                //

                var xaxisg = svg.append("g")
                    .attr("class", "d3-axis d3-axis-horizontal")
                    .attr("transform", "translate(0," + height + ")")
                    .call(xAxis);

                // Add extra subticks for hidden hours
                xaxisg.selectAll(".d3-axis-subticks")
                    .data(x.ticks(d3.time.hours), function(d) { return d; })
                    .enter()
                    .append("line")
                    .attr("class", "d3-axis-subticks")
                    .attr("y1", 0)
                    .attr("y2", 4)
                    .attr("x1", x)
                    .attr("x2", x);



                // Add hover line and pointer
                // ------------------------------

                // Append group to the group of paths to prevent appearance outside chart area
                var hoverLineGroup = group.append("g")
                    .attr("class", "hover-line");

                // Add line
                var hoverLine = hoverLineGroup
                    .append("line")
                    .attr("class", "d3-crosshair-line")
                    .attr("y1", 0)
                    .attr("y2", height)
                    .style("opacity", 0);

                // Add pointer
                var hoverPointer = hoverLineGroup
                    .append("rect")
                    .attr("class", "d3-crosshair-line")
                    .attr("x", 2)
                    .attr("y", 2)
                    .attr("width", 6)
                    .attr("height", 6)
                    .style('fill', '#03A9F4')
                    .style("opacity", 0);



                // Append events to the layers group
                // ------------------------------

                layerTransition.each("end", function() {
                    layer
                        .on("mouseover", function (d, i) {
                            svg.selectAll(".streamgraph-layer")
                                .transition()
                                .duration(250)
                                .style("opacity", function (d, j) {
                                    return j != i ? 0.75 : 1; // Mute all except hovered
                                });
                        })

                        .on("mousemove", function (d, i) {
                            mouse = d3.mouse(this);
                            mousex = mouse[0];
                            mousey = mouse[1];
                            datearray = [];
                            var invertedx = x.invert(mousex);
                            invertedx = invertedx.getHours();
                            var selected = (d.values);
                            for (var k = 0; k < selected.length; k++) {
                                datearray[k] = selected[k].date
                                datearray[k] = datearray[k].getHours();
                            }
                            mousedate = datearray.indexOf(invertedx);
                            pro = d.values[mousedate].value;


                            // Display mouse pointer
                            hoverPointer
                                .attr("x", mousex - 3)
                                .attr("y", mousey - 6)
                                .style("opacity", 1);

                            hoverLine
                                .attr("x1", mousex)
                                .attr("x2", mousex)
                                .style("opacity", 1);

                            //
                            // Tooltip
                            //

                            // Tooltip data
                            tooltip.html(
                                '<ul class="list-unstyled mb-1 p-0">' +
                                    '<li>' + '<div class="fs-base my-1"><i class="ph-arrow-circle-left"></i><span class="d-inline-block ms-2"></span>' + d.key + '</div>' + '</li>' +
                                    '<li>' + 'Visits: &nbsp;' + "<span class='fw-semibold float-end'>" + pro + '</span>' + '</li>' +
                                    '<li>' + 'Time: &nbsp; ' + '<span class="fw-semibold float-end">' + formatDate(d.values[mousedate].date) + '</span>' + '</li>' + 
                                '</ul>'
                            )
                            .style("display", "block");

                            // Tooltip arrow
                            tooltip.append('div').attr('class', 'd3-tip-arrow');
                        })

                        .on("mouseout", function (d, i) {

                            // Revert full opacity to all paths
                            svg.selectAll(".streamgraph-layer")
                                .transition()
                                .duration(250)
                                .style("opacity", 1);

                            // Hide cursor pointer
                            hoverPointer.style("opacity", 0);

                            // Hide tooltip
                            tooltip.style("display", "none");

                            hoverLine.style("opacity", 0);
                        });
                    });



                // Append events to the chart container
                // ------------------------------

                d3Container
                    .on("mousemove", function (d, i) {
                        mouse = d3.mouse(this);
                        mousex = mouse[0];
                        mousey = mouse[1];

                        // Move tooltip vertically
                        tooltip.style("top", (mousey - (this.querySelector('.d3-tip').getBoundingClientRect().height / 2)) - 2 + "px") // Half tooltip height - half arrow width

                        // Move tooltip horizontally
                        if(mousex >= (this.getBoundingClientRect().width - this.querySelector('.d3-tip').getBoundingClientRect().width - margin.right - (tooltipOffset * 2))) {
                            tooltip
                                .style("left", (mousex - this.querySelector('.d3-tip').getBoundingClientRect().width - tooltipOffset) + "px") // Change tooltip direction from right to left to keep it inside graph area
                                .attr("class", "d3-tip w");
                        }
                        else {
                            tooltip
                                .style("left", (mousex + tooltipOffset) + "px" )
                                .attr("class", "d3-tip e");
                        }
                    });
            });



            // Resize chart
            // ------------------------------

            // Call function on window resize
            var resizeStreamTimer;
            window.addEventListener('resize', function() {
                clearTimeout(resizeStreamTimer);
                resizeStreamTimer = setTimeout(function () {
                    resizeStream();
                }, 200);
            });

            // Call function on sidebar width change
            var sidebarToggle = document.querySelectorAll('.sidebar-control');
            if (sidebarToggle) {
                sidebarToggle.forEach(function(togglers) {
                    togglers.addEventListener('click', resizeStream);
                });
            }

            // Resize function
            // 
            // Since D3 doesn't support SVG resize by default,
            // we need to manually specify parts of the graph that need to 
            // be updated on window resize
            function resizeStream() {

                // Layout
                // -------------------------

                // Define width
                width = d3Container.node().getBoundingClientRect().width - margin.left - margin.right;

                // Main svg width
                container.attr("width", width + margin.left + margin.right);

                // Width of appended group
                svg.attr("width", width + margin.left + margin.right);

                // Horizontal range
                x.range([0, width]);


                // Chart elements
                // -------------------------

                // Horizontal axis
                svg.selectAll('.d3-axis-horizontal').call(xAxis);

                // Horizontal axis subticks
                svg.selectAll('.d3-axis-subticks').attr("x1", x).attr("x2", x);

                // Grid lines width
                svg.selectAll(".d3-grid-dashed").call(gridAxis.tickSize(-width, 0, 0))

                // Right vertical axis
                svg.selectAll(".d3-axis-right").attr("transform", "translate(" + width + ", 0)");

                // Area paths
                svg.selectAll('.streamgraph-layer').attr("d", function(d) { return area(d.values); });
            }
        }
    };


    //
    // Return objects assigned to module
    //

    return {
        init: function() {
            _streamgraph();
        }
    }
}();


// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function() {
    D3Streamgraph.init();
});

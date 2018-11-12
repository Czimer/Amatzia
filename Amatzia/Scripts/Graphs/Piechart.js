function CreatePiechart() {
    $.ajax(
        {
            type: 'GET',
            url: 'Stats/GetGenderAndPositiveComments',
            success: function (json) {

                //var data = [
                //    { name: "USA", value: 60 },
                //    { name: "UK", value: 20 },
                //    { name: "Canada", value: 30 },
                //    { name: "Maxico", value: 15 },
                //    { name: "Japan", value: 10 },
                //];
                var text = "";

                var width = 400;
                var height = 400;
                var thickness = 40;
                var duration = 750;
                var padding = 10;
                var opacity = .8;
                var opacityHover = 1;
                var otherOpacityOnHover = .8;
                var tooltipMargin = 13;

                var radius = Math.min(width - padding, height - padding) / 2;
                var color = d3.scaleOrdinal(d3.schemeCategory10);

                var svg = d3.select("#pieSvg")
                    .append('svg')
                    .attr('class', 'pie')
                    .attr('width', width)
                    .attr('height', height);

                var g = svg.append('g')
                    .attr('transform', 'translate(' + (width / 2) + ',' + (height / 2) + ')');

                var arc = d3.arc()
                    .innerRadius(0)
                    .outerRadius(radius);

                var pie = d3.pie()
                    .value(function (d) { return d.count; })
                    .sort(null);

                var path = g.selectAll('path')
                    .data(pie(json))
                    .enter()
                    .append("g")
                    .append('path')
                    .attr('d', arc)
                    .attr('fill', (d, i) => color(i))
                    .style('opacity', opacity)
                    .style('stroke', 'white')
                    .on("mouseover", function (d) {
                        d3.selectAll('path')
                            .style("opacity", otherOpacityOnHover);
                        d3.select(this)
                            .style("opacity", opacityHover);

                        let g = d3.select("svg")
                            .style("cursor", "pointer")
                            .append("g")
                            .attr("class", "tooltip")
                            .style("opacity", 0);

                        g.append("text")
                            .attr("class", "gender-text")
                            .text(`${d.data.gender} (${d.data.count})`)
                            .attr('text-anchor', 'middle');

                        let text = g.select("text");
                        let bbox = text.node().getBBox();
                        let padding = 2;
                        g.insert("rect", "text")
                            .attr("x", bbox.x - padding)
                            .attr("y", bbox.y - padding)
                            .attr("width", bbox.width + (padding * 2))
                            .attr("height", bbox.height + (padding * 2))
                            .style("fill", "white")
                            .style("opacity", 0.75);
                    })
                    .on("mousemove", function (d) {
                        let mousePosition = d3.mouse(this);
                        let x = mousePosition[0] + width / 2;
                        let y = mousePosition[1] + height / 2 - tooltipMargin;

                        let text = d3.select('.tooltip text');
                        let bbox = text.node().getBBox();
                        if (x - bbox.width / 2 < 0) {
                            x = bbox.width / 2;
                        }
                        else if (width - x - bbox.width / 2 < 0) {
                            x = width - bbox.width / 2;
                        }

                        if (y - bbox.height / 2 < 0) {
                            y = bbox.height + tooltipMargin * 2;
                        }
                        else if (height - y - bbox.height / 2 < 0) {
                            y = height - bbox.height / 2;
                        }

                        d3.select('.tooltip')
                            .style("opacity", 1)
                            .attr('transform', `translate(${x}, ${y})`);
                    })
                    .on("mouseout", function (d) {
                        d3.select("svg")
                            .style("cursor", "none")
                            .select(".tooltip").remove();
                        d3.selectAll('path')
                            .style("opacity", opacity);
                    })
                    .on("touchstart", function (d) {
                        d3.select("svg")
                            .style("cursor", "none");
                    })
                    .each(function (d, i) { this._current = i; });

                let legend = d3.select("#pieSvg").append('div')
                    .attr('class', 'legend')
                    .style('margin-top', '30px');

                let keys = legend.selectAll('.key')
                    .data(json)
                    .enter().append('div')
                    .attr('class', 'key')
                    .style('display', 'flex')
                    .style('align-items', 'center')
                    .style('margin-right', '20px');

                keys.append('div')
                    .attr('class', 'symbol')
                    .style('height', '10px')
                    .style('width', '10px')
                    .style('margin', '5px 5px')
                    .style('background-color', (d, i) => color(i));

                keys.append('div')
                    .attr('class', 'gender')
                    .text(d => `${d.gender} (${d.count})`);

                keys.exit().remove();

            }
        });
}

CreatePiechart();


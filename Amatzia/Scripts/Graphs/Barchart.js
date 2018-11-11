function CreateBarchart() {
    $.ajax(
        {
            type: 'GET',
            url: 'User/GetUsersByCountry',
            success: function (json) {
                const svg = d3.select('svg');
                const svgContainer = d3.select('#container');

                const margin = 80;
                const width = 1000 - 2 * margin;
                const height = 600 - 2 * margin;

                const chart = svg.append('g')
                    .attr('transform', `translate(${margin}, ${margin})`);

                const xScale = d3.scaleBand()
                    .range([0, width])
                    .domain(json.map((s) => s.country))
                    .padding(0.4)

                const yScale = d3.scaleLinear()
                    .range([height, 0])
                    .domain([0, Math.max.apply(this, json.map(s => s.count)) + 10]);

                const makeYLines = () => d3.axisLeft()
                    .scale(yScale)

                chart.append('g')
                    .attr('transform', `translate(0, ${height})`)
                    .call(d3.axisBottom(xScale));

                chart.append('g')
                    .call(d3.axisLeft(yScale));

                chart.append('g')
                    .attr('class', 'grid')
                    .call(makeYLines()
                        .tickSize(-width, 0, 0)
                        .tickFormat('')
                    )

                const barGroups = chart.selectAll()
                    .data(json)
                    .enter()
                    .append('g')

                barGroups
                    .append('rect')
                    .attr('class', 'bar')
                    .attr('x', (g) => xScale(g.country))
                    .attr('y', (g) => yScale(g.count))
                    .attr('height', (g) => height - yScale(g.count))
                    .attr('width', xScale.bandwidth())
                    .on('mouseenter', function (actual, i) {
                        d3.selectAll('.count')
                            .attr('opacity', 0)

                        d3.select(this)
                            .transition()
                            .duration(300)
                            .attr('opacity', 0.6)
                            .attr('x', (a) => xScale(a.country) - 5)
                            .attr('width', xScale.bandwidth() + 10)

                        const y = yScale(actual.count)

                        line = chart.append('line')
                            .attr('id', 'limit')
                            .attr('x1', 0)
                            .attr('y1', y)
                            .attr('x2', width)
                            .attr('y2', y)

                        barGroups.append('text')
                            .attr('class', 'divergence')
                            .attr('x', (a) => xScale(a.country) + xScale.bandwidth() / 2)
                            .attr('y', (a) => yScale(a.count) + 30)
                            .attr('fill', 'white')
                            .attr('text-anchor', 'middle')
                            .text((a, idx) => {
                                const divergence = (a.count - actual.count).toFixed(1)

                                let text = ''
                                if (divergence > 0) text += '+'
                                text += `${divergence}%`

                                return idx !== i ? text : '';
                            })

                    })
                    .on('mouseleave', function () {
                        d3.selectAll('.count')
                            .attr('opacity', 1)

                        d3.select(this)
                            .transition()
                            .duration(300)
                            .attr('opacity', 1)
                            .attr('x', (a) => xScale(a.country))
                            .attr('width', xScale.bandwidth())

                        chart.selectAll('#limit').remove()
                        chart.selectAll('.divergence').remove()
                    })

                barGroups.append('text')
                    .attr('class', 'value')
                    .attr('x', (a) => xScale(a.country) + xScale.bandwidth() / 2)
                    .attr('y', (a) => yScale(a.count) + 30)
                    .attr('text-anchor', 'middle')
                    .text((a) => a.count)

                svg.append('text')
                    .attr('class', 'label')
                    .attr('x', -(height / 2) - margin)
                    .attr('y', margin / 2.4)
                    .attr('transform', 'rotate(-90)')
                    .attr('text-anchor', 'middle')
                    .text('Number of users')

                svg.append('text')
                    .attr('class', 'label')
                    .attr('x', width / 2 + margin)
                    .attr('y', height + margin * 1.7)
                    .attr('text-anchor', 'middle')
                    .text('Countries')

                svg.append('text')
                    .attr('class', 'title')
                    .attr('x', width / 2 + margin)
                    .attr('y', 40)
                    .attr('text-anchor', 'middle')
                    .text('Users & Countries')

            }
        });
}

CreateBarchart();


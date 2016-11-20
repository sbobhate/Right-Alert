$(document).ready(function LoadDefault() {

        if ($('.pages_dashboard').length) {
       
            var dayOfWeek = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
            var d = new Date();
            var n = d.getDay();
            n = n + 7;

            //document.getElementById  // use this process later
            // created invisible html elements
            var yvals = [$("#sd0").val(), $("#sd1").val(), $("#sd2").val(), $("#sd3").val(), $("#sd4").val(), $("#sd5").val(), $("#sd6").val()];

            if ($('.ld-widget-main__chart').length) {
                Morris.Bar({
                    element: $('.ld-widget-main__chart'),
                    data: [
                        { day: dayOfWeek[n - 6], y: yvals[6] },
                        { day: dayOfWeek[n - 5], y: yvals[5] },
                        { day: dayOfWeek[n - 4], y: yvals[4] },
                        { day: dayOfWeek[n - 3], y: yvals[3] },
                        { day: dayOfWeek[n - 2], y: yvals[2] },
                        { day: dayOfWeek[n - 1], y: yvals[1] },
                        { day: dayOfWeek[n], y: yvals[0] },
                    ],
                    xkey: 'day',
                    ykeys: ['y'],
                    labels: ['Bikers'],
                    barColors: ['#20c05c'],
                    stacked: true,
                    resize: true
                });
            }

            if ($('.ld-widget-side__chart').length) {
                Morris.Bar({
                    element: $('.ld-widget-main__chart'),
                    data: [
                        { y: '2012', a: 40, b: 50 },
                        { y: '2011', a: 75, b: 65 },
                        { y: '2010', a: 100, b: 90 },
                        { y: '2009', a: 75, b: 65 },
                        { y: '2008', a: 50, b: 40 },
                    ],
                    xkey: 'y',
                    ykeys: ['a', 'b'],
                    labels: ['Item A', 'Item B'],
                    barColors: ['#20c05c', '#FF5722'],
                    stacked: true,
                    resize: true
                });
            }

            $('.selectpicker').selectpicker();
    }

});
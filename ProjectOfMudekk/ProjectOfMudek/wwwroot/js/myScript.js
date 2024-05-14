$(document).ready(function () {
    $('#cityDropdown').change(function () {
        var cityId = $(this).val();
        $.ajax({
            type: 'POST',
            url: '/Select/GetDistricts',
            data: { cityId: cityId },
            success: function (districts) {
                $('#districtDropdown').empty();
                $.each(districts, function (i, district) {
                    $('#districtDropdown').append($('<option>').val(district.id).text(district.facultyName));
                });
            }
        });
    });
});
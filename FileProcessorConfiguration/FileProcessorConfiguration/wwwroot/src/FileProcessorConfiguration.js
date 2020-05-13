<script>
    $(document).ready(function () {
        $.get("api/ClientInfoes", function (data, status) {
            for (let c of data) {
                $("#ClientInfo").append(new Option(c.clientAliasName, c));
            }
        });

    $("#upload").click(function () {

    });
});


    </script>
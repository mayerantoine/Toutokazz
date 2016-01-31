$(document).ready(function () {

    function initializeDeposerannonce() {
        //hide voiture
        $('#marque_vehicule').hide();
        $('#modele_vehicule').hide();
        $('#annee_vehicule').hide();
        $('#transmission').hide();
        $('#carburant').hide();
        $('#mileage').hide();
        $('#condition').show();

        //hide immobilier
        $('#nbpieces').hide();
        $('#nbchambre').hide();
        $('#surface').hide();
        $('#loyer').hide();

        //hide chaussure
        $('#marque_chaussure').hide();
        $('#chaussure_taille').hide();

    };

    //initialize depot
    initializeDeposerannonce();

    //initialize create account
    function init_create_account() {

        console.log('change :' + $("#type_utilisateur input[name=account_type]:checked").val());

        if ($("#type_utilisateur input[name=account_type]:checked").val() == 2) {
            $('#f_nom_entreprise').show();
        }
        else {
            $('#f_nom_entreprise').hide();
        }

    }

    init_create_account();

   

    // Drop down menu handler
    $('.dropdown-menu').find('form').click(function (e) {
        e.stopPropagation();
    });

    $('.carousel').carousel({
        interval: 5000
    })

    // Slider
    $("#slides").slidesjs({
        width: 900,
        height: 300,
        navigation: false,
        play: {
            active: false,
            effect: "slide",
            interval: 4000,
            auto: true,
            swap: false,
            pauseOnHover: true,
            restartDelay: 2500
        }
    }); //end slide

    $('.list-subgroups').hide();
    $('.list-group-item').click(function () {
        console.log($(this));
        $(this).siblings().next('div.list-subgroups').slideUp('200');
        $(this).next('div.list-subgroups').slideToggle('200');

    });
    var readURL = function (input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                var imgpreview = $(input).data("preview");
                var img = document.getElementById(imgpreview);
                console.log("load img");
                console.log(imgpreview);
                img.setAttribute("src", e.target.result);
                //$("#"+imgpreview).attr('src', e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
    }

    $(".preview").change(function () {
   //     console.log("change");
        readURL(this);
    });


    var options = {
        target: "#output",
        dataType:"Json",
        beforeSubmit: beforeSubmit,
        success: afterSuccess,
        resetForm: false
    };

    $('#UploadForm').submit(function (e) {
        e.preventDefault(); //STOP default action
        $(this).ajaxSubmit(options);
        return false;
    });

    function beforeSubmit() {
        $('#loading-img').show(); //hide submit button
        $('#loading-msg').hide();
    };

    function afterSuccess(data) {
        $('#loading-img').hide(); //hide submit button
        $('#loading-msg').slideDown(500);
        if (data == "OK") {
            document.getElementById("UploadForm").reset();
            $('#loading-msg h4').text("Votre annonce a été soumise avec succes.");
           // window.location.href = "http://localhost:56713/MesAnnonces/Index";
            window.location.href = "http://toutokazz.com/MesAnnonces/Index";
          //   window.location.href = "http://staging-toutokazz.azurewebsites.net/MesAnnonces/Index";
        }
        else {
            $('#loading-msg h4').text(data);
        }
    };


    // Add visit departement master-childpage
    $('#modelannonce_id_departement').change(function () {
 
        $.ajax({
            url: '/annonces/getCommuneByDepartement/' + $('select#modelannonce_id_departement').val(),
            async: true,
            type: "get",
            datatype: "Json",
            success: function (data) {
                var items = '<option value='+''+'>Choisir Commune</option>';
                $.each(data, function (i, selectedsite) {
                //    console.log(selectedsite.Value);
                    items += '<option value=' + selectedsite.Value + '>' + selectedsite.Text + '</option>';
                });
                console.log(items);
                $('select#modelannonce_id_commune').html(items);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(jqXHR + "-" + textStatus + "-" + errorThrown);
            },
        });
     
    });
    
    // Check all ads
    $('#chkall').change(function () {

        if ($(this).is(':checked')) {
            console.log("chek all..");
          
            $('input:checkbox').attr('checked', true);
            console.log($('input:checkbox'));

        }
        else {
            console.log(" unchek all....");
            $('input:checkbox').attr('checked', false);
        }

    });

    $('.chkbox').change(function () {
        if ($(this).is(':checked')) {
        }
        else {
            if ($('#chkall').is(':checked')) {
                $('#chkall').attr('checked', false);
            }
        }

    });

    $('#btndesactiver').click(function () {
        $('.chkbox').each(function () {
            if ($(this).is(':checked')) {
                // alert($(this).attr('id'));


            }

        });

    }); // end button  desactiver click

    $('#modalDeleteAds').on('show', function () {
        var id = $(this).data('ads'),
            removeBtn = $(this).find('.danger');
    });

    $('#modalDesactiverAds').on('show', function () {
        var id = $(this).data('ads'),
            removeBtn = $(this).find('.danger');
    });

    $('#modalVenduAds').on('show', function () {
        var id = $(this).data('ads'),
            removeBtn = $(this).find('.danger');
    });

    $('#modalPublishedAds').on('show', function () {
        var id = $(this).data('ads'),
            removeBtn = $(this).find('.danger');
    });

    $('.confirm-delete').on('click', function (e) {
        e.preventDefault();

        var id = $(this).data('ads');
        var href = $(this).attr('href');
        console.log(href);
        $('#btnYes').attr('href', href);
        $('#modalDeleteAds').data('ads', id).modal('show');
        return false;

    });

    $('.confirm-desactiver').on('click', function (e) {
        e.preventDefault();
        console.log("desactiver");
        var id = $(this).data('ads');
        var href = $(this).attr('href');
        console.log(href);
        $('#btnDesactiverYes').attr('href', href);
        $('#modalDesactiverAds').data('ads', id).modal('show');
        return false;
    });
    

    $('.confirm-publier').on('click', function (e) {
        e.preventDefault();
        console.log("publier");
        var id = $(this).data('ads');
        var href = $(this).attr('href');
        console.log(href);
        $('#btnPublierYes').attr('href', href);
        $('#modalPublishedAds').data('ads', id).modal('show');
        return false;

    });

    $('.confirm-vendu').on('click', function (e) {
        e.preventDefault();

        var id = $(this).data('ads');
        var href = $(this).attr('href');
        console.log(href);
        $('#btnVenduYes').attr('href', href);
        $('#modalVenduAds').data('ads', id).modal('show');
        return false;

    });

    $('#main-right').on("click", "#btnParticulier", function () {
        console.log("click");
       
        $.ajax({
            url: '/annonces/recherche?id_account_type=1',
            async: true,
            type: "get",
            success: function (data) {
                $('#main-right').html(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(jqXHR + "-" + textStatus + "-" + errorThrown);
            },

        });
       
    });
    $('#main-right').on("click", "#btnPro", function () {
        console.log("click");
        $.ajax({
            url: '/annonces/recherche?id_account_type=2',
            async: true,
            type: "get",
            success: function (data) {
                $('#main-right').html(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(jqXHR + "-" + textStatus + "-" + errorThrown);
            },

        });

    });

    $("#main-right").on("click", "#btnToutes", function () {
        console.log("click");
        $.ajax({
            url: '/annonces/recherche',
            async: true,
            type: "get",
            success: function (data) {
                $('#main-right').html(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(jqXHR + "-" + textStatus + "-" + errorThrown);
            },

        });

    });
    $(".help-block").hide();
    $("#password").focus(function () {
        $(".help-block").show();

    }).blur(function () {
        $(".help-block").hide();
    });

    $("#confirmPassword").blur(function () {
        var $p1 = $(this);
        var p1 = document.getElementById("confirmPassword");
        var $p2 = $("#password");
        var p2 = document.getElementById("password");
        console.log($p1.val());
        if (p1.value != p2.value || p1.value == '' || p2.value == '') {
            p2.setCustomValidity('Votre mot de passe est incorrect');
        } else {
            p2.setCustomValidity('');
        }

    });

    function onBeginContact() {

    };
    function onCompleteContact() {

    };
    function onFailureContact() {

    };
    // Drop down menu handler
    $('.dropdown-menu').find('form').click(function (e) {
        e.stopPropagation();
    });
    // Carousel (slider)
    $('#detailCarousel').carousel({
        interval: 15000
    });
    $('[id^=carousel-selector-]').click(function () {
        var id_selector = $(this).attr("id");
        var id = id_selector.substr(id_selector.length - 1);
        id = parseInt(id);
        $('#detailCarousel').carousel(id);

        console.log("slide  :" + id);
        $('[id^=carousel-selector-]').removeClass('selected');
        $(this).addClass('selected');
    });
    $('#detailCarousel').on('slid', function (e) {
        var id = $('.item.active').data('slide-number');
        console.log("slide  :" + id);
        id = parseInt(id);
        $('[id^=carousel-selector-]').removeClass('selected');
        $('[id^=carousel-selector-' + id + ']').addClass('selected');
    });

    var SPHideShowField = {
        '1': function () {
            //voiture
            $('#marque_vehicule').show();
            $('#modele_vehicule').show();
            $('#annee_vehicule').show();
            $('#transmission').show();
            $('#carburant').show();
            $('#mileage').show();
            $('#condition').show();
            $('#prix').show();

            //hide immobilier
            $('#nbpieces').hide();
            $('#nbchambre').hide();
            $('#surface').hide();
            $('#loyer').hide();

            //hide chaussure
            $('#marque_chaussure').hide();
            $('#chaussure_taille').hide();

        },
        '2': function () {
            //moto
            $('#marque_vehicule').show();
            $('#modele_vehicule').show();
            $('#annee_vehicule').show();
            $('#transmission').hide();
            $('#carburant').show();
            $('#mileage').hide();
            $('#condition').show();
            $('#prix').show();

            //hide immobilier
            $('#nbpieces').hide();
            $('#nbchambre').hide();
            $('#surface').hide();
            $('#loyer').hide();

            //hide chaussure
            $('#marque_chaussure').hide();
            $('#chaussure_taille').hide();
        },
        '66': function () {
            //camion engin lourd
            $('#marque_vehicule').show();
            $('#modele_vehicule').hide();
            $('#annee_vehicule').show();
            $('#transmission').hide();
            $('#carburant').hide();
            $('#mileage').hide();
            $('#condition').show();
            $('#prix').show();

            //hide immobilier
            $('#nbpieces').hide();
            $('#nbchambre').hide();
            $('#surface').hide();
            $('#loyer').hide();

            //hide chaussure
            $('#marque_chaussure').hide();
            $('#chaussure_taille').hide();
        },
        '4': function () {
            //voiture
            $('#marque_vehicule').show();
            $('#modele_vehicule').show();
            $('#annee_vehicule').show();
            $('#transmission').show();
            $('#carburant').show();
            $('#mileage').show();
            $('#condition').hide();
            $('#prix').show();

            //hide immobilier
            $('#nbpieces').hide();
            $('#nbchambre').hide();
            $('#surface').hide();
            $('#loyer').hide();

            //hide chaussure
            $('#marque_chaussure').hide();
            $('#chaussure_taille').hide();

        },
        '71': function () {
            //voiture
            $('#marque_vehicule').hide();
            $('#modele_vehicule').hide();
            $('#annee_vehicule').hide();
            $('#transmission').hide();
            $('#carburant').hide();
            $('#mileage').hide();

            //
            $('#condition').hide();
            $('#prix').show();

            //hide immobilier
            $('#nbpieces').hide();
            $('#nbchambre').hide();
            $('#surface').show();
            $('#loyer').hide();

            //hide chaussure
            $('#marque_chaussure').hide();
            $('#chaussure_taille').hide();

        },
        'vendre': function () {
            //voiture
            $('#marque_vehicule').hide();
            $('#modele_vehicule').hide();
            $('#annee_vehicule').hide();
            $('#transmission').hide();
            $('#carburant').hide();
            $('#mileage').hide();

            //
            $('#condition').show();
            $('#prix').show();

            //hide immobilier
            $('#nbpieces').show();
            $('#nbchambre').show();
            $('#surface').show();
            $('#loyer').hide();

            //hide chaussure
            $('#marque_chaussure').hide();
            $('#chaussure_taille').hide();

        },
        'louer': function () {
            //voiture
            $('#marque_vehicule').hide();
            $('#modele_vehicule').hide();
            $('#annee_vehicule').hide();
            $('#transmission').hide();
            $('#carburant').hide();
            $('#mileage').hide();

            //
            $('#condition').show();
            $('#prix').hide();

            //hide immobilier
            $('#nbpieces').show();
            $('#nbchambre').show();
            $('#surface').show();
            $('#loyer').show();

            //hide chaussure
            $('#marque_chaussure').hide();
            $('#chaussure_taille').hide();

        },
        'service': function () {

            //hide voiture
            $('#marque_vehicule').hide();
            $('#modele_vehicule').hide();
            $('#annee_vehicule').hide();
            $('#transmission').hide();
            $('#carburant').hide();
            $('#mileage').hide();

            $('#condition').hide();
            $('#prix').hide();

            //hide immobilier
            $('#nbpieces').hide();
            $('#nbchambre').hide();
            $('#surface').hide();
            $('#loyer').hide();

            //hide chaussure
            $('#marque_chaussure').hide();
            $('#chaussure_taille').hide();

        },
        'default': function () {
            //hide voiture
            $('#marque_vehicule').hide();
            $('#modele_vehicule').hide();
            $('#annee_vehicule').hide();
            $('#transmission').hide();
            $('#carburant').hide();
            $('#mileage').hide();

            $('#condition').hide();
            $('#prix').show();

            //hide immobilier
            $('#nbpieces').hide();
            $('#nbchambre').hide();
            $('#surface').hide();
            $('#loyer').hide();

            //hide chaussure
            $('#marque_chaussure').hide();
            $('#chaussure_taille').hide();
        }      

    };

    // deposer annonce form select field
    $('#modelannonce_id_category').change(function () {
        var selected = $('select#modelannonce_id_category').val();
     

        if (selected == 1) {
            //voiture
            SPHideShowField[selected]();
        }
        else if (selected == 2) {
            SPHideShowField[selected]();

        }
        else if (selected == 66) {
            SPHideShowField[selected]();

        }
        else if (selected == 4) {
            SPHideShowField[selected]();
        }
        else if (selected >= 45 && selected <= 61) {
            //services
            SPHideShowField['service']();
        }
        else if(selected == 71){
            SPHideShowField[selected]();
        }
        else if(selected == 5 || selected == 63){
            // a vendre
            SPHideShowField['vendre']();
        }
        else if ((selected >=6 && selected <= 9) || selected == 64) {
            // a louer
            SPHideShowField['louer']();
        }
        else {
            SPHideShowField['default']();
        }
  
      }); // end select category




    //create account type selected

    $("#type_utilisateur input[name=account_type]:radio").change(function () {

        console.log('change');
        console.log($(this).val());
        if ($(this).val() == 1 ) {
            $('#f_nom_entreprise').hide();
        }
        if ($(this).val() == 2) {
            console.log('2 is check')
            $('#f_nom_entreprise').show();
        }

    });


    //details page hide and show ad phone
    $("#ad_phone").hide();
    $("#afficher").on('click',function (e) {
        e.preventDefault();

        console.log("click afficher");
        $("#ad_phone").show();
        $("#afficher").hide();
    });




}); //end ready
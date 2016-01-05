
$(function () {

    //section
    var section = function (Id, section_title,icon_image,categories) {
        this.id_section = ko.observable(Id);
        this.section_title = ko.observable(section_title);
        categories = typeof(categories) !== 'undefined' ? categories : [];
        this.categories = ko.observableArray(categories);
    }

    //category
    var category = function (Id, category_title) {
        this.id_category = ko.observable(Id);
        this.category_title = ko.observable(category_title);
    }

    // new extensions fields
    var fields = function (id, field_name) {
        this.id_field =ko.observable(id);
        this.field_name = ko.observable(field_name);
    }

    //departement
    var departement = function (id_department, departement) {
        this.id_department = ko.observable(id_department);
        this.departement = ko.observable(departement);
        communes = typeof (communes) !== 'undefined' ? communes : [];

    }

    //commune
    var commune = function (id_commune, commune, id_department) {
        this.id_commune = ko.observable(id_commune);
        this.commune = ko.observable(commune);
        this.id_department = ko.observable(id_department);
    }

    //type annonce
    var type = function (id_ad_type,description) {
        this.id_ad_type = ko.observable(id_ad_type);
        this.description = ko.observable(description);
    }

    // account
    var account_type = function (id_account_type,account_title) {
        this.id_account_type = ko.observable(id_account_type);
        this.account_title = ko.observable(account_title);
    }

    // transmission
    var transmission = function (id_transmission, transmission) {
        this.id_transmission = ko.observable(id_transmission);
        this.transmission = ko.observable(transmission);
    }

    //devise
    var devise = function (id_devise,devise) {
        this.id_devise = ko.observable(id_devise);
        this.devise = ko.observable(devise);
    }

     // condition etat
    var item_condition = function (id_item_condition,condition) {
        this.id_item_condition = ko.observable(id_item_condition);
        this.condition = ko.observable(condition);
    }

    // essence
    var carburant = function (id_carburant, carburant) {
        this.id_carburant = ko.observable(id_carburant);
        this.carburant = ko.observable(carburant)
    };

    ko.bindingHandlers.option = {
        update: function (element, valueAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor());
            ko.selectExtensions.writeValue(element, value);
        }
    };

    var appViewModel = function () {
        var self = this;

        self.categoryBySection = ko.observableArray([]);
        self.departments = ko.observableArray([])
        self.communes = ko.observableArray([]);
        self.ads_type = ko.observableArray(
            [new type("1", "Vous Vendez"),new type("2", "Vous achetez")]);
        self.account_type = ko.observableArray([new account_type("1", "Particulier"), new account_type("1", "Professionel")]);
        self.devise = ko.observableArray([new devise("1","Gourdes"),new devise("2","USD")]);
        self.transmission = ko.observableArray([]);
        self.condition = ko.observableArray([new item_condition("1", "Neuf"), new item_condition("3", "d'occasion")]);
        self.carburant = ko.observable([new carburant("1","Gazoline"),new carburant("2","Diesel"),new carburant("3","Propane")]);

        
        self.selected_section = ko.observable();
        self.selected_category = ko.computed(function () {
            var selected = this.selected_section();
            return selected ? selected.id_category() : 'unknown';
        }, self);
        self.selected_department = ko.observable();
        self.selected_commune = ko.observable();
        self.selected_ads_type = ko.observable();
        self.selected_account_type = ko.observable();
        self.selected_devise = ko.observable();
        self.prix = ko.observable();
    
        self.selected_condition = ko.observable();
        self.selected_transmission = ko.observable();
        self.marque_vehicule = ko.observable();
        self.modele_vehicule = ko.observable();
        self.annee_vehicule = ko.observable();
        self.selected_carburant = ko.observable();
        self.selected_etat = ko.observable();
        self.mileage_vehicule = ko.observable();

        self.ad_title = ko.observable();
        self.ad_description = ko.observable();

        self.ad_phone = ko.observable();
        self.email = ko.observable();
        self.password = ko.observable();
        self.confirm_password = ko.observable();


        self.communesByDepartment = ko.computed(function () {
            if (!self.selected_department()) { return null; }
            var filter = self.selected_department();
        //    console.log(filter.id_department());
            return ko.utils.arrayFilter(self.communes(), function (item) {
                return item.id_department() === filter.id_department();
            });
        },self);

      
        self.saveUserAds = function () {
         //   console.log(ko.toJSON(self));
        };


        self.loadData = function () {
            $.getJSON("/annonces/getJSONData", function (data) {
               // console.log(data);

                //section & category
                $.each(data[0].ad_category, function (i, p) {
             //     console.log(p.section_title);
                    var s = new section(p.id_section, p.section_title);
                    $.each(p.catList, function (i, p) {
                        var c = new category(p.id_category, p.category_title);
                        s.categories.push(c);
                    });
                    self.categoryBySection.push(s);
                });

                //department
                $.each(data[1].departement, function (i, p) {
                    var d = new departement(p.id_departement, p.departement);
                    self.departments.push(d);
                });

                //commune
                $.each(data[2].commune, function (i, p) {
                    var com = new commune(p.id_commune, p.commune, p.id_departement);
                   // console.log(p.commune)
                    self.communes.push(com);
                });

                //transmission
                $.each(data[3].transmission, function (i, p) {
                    var t = new transmission(p.ref_item_id, p.ref_item);
                  //  console.log(p.ref_item)
                    self.transmission.push(t);
                });

              
            });
        };

     };
    var vm = new appViewModel();
    vm.loadData();
    ko.applyBindings(vm);
}())

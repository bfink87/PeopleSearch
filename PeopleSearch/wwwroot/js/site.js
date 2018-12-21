new Vue({
    el: '#app',
    data() {
        return {
            query: null,
            people: null,
            loading: false,
            errored: false,
        }
    }, 
    // TODO: add CRUD methods to allow adding and updating people from interface
    methods: {
        search: function () {
            var endpoint = '/api/People/';

            if (this.query != null)
            {
                endpoint = '/api/People/query=' + this.query;
            }
            this.loading = true;
            axios
                .get(endpoint)
                .then(response => {
                    this.people = response.data
                })
                .catch(error => {
                    console.log(error)
                    this.errored = true
                })
                .finally(() => this.loading = false)
        }
    }
})
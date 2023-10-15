namespace BarcodeService.Application.Models;


    public class OpenApiResponse
    {
        public string code { get; set; }
        public object[] errors { get; set; }
        public ProductFromApi product { get; set; }
        public Result result { get; set; }
        public object[] warnings { get; set; }
        public string status { get; set; }
    }

    public class ProductFromApi
    {
        public string product_name { get; set; }
        public string brands { get; set; }
    }

    public class Result
    {
        public string id { get; set; }
        public string lc_name { get; set; }
        public string name { get; set; }
    }
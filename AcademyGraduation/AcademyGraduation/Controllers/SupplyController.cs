
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;

namespace AcademyGraduationSupplyManagement.Controllers
{
    // Supply Offer Class
    public class SupplyOffer
    {
        public string SupplierName { get; set; } = "";
        public string OfferName { get; set; } = "";
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class SupplyController : Controller
    {
        // Category 1: Graduation Certificates
        private List<SupplyOffer> certificates = new List<SupplyOffer>
        {
            new SupplyOffer
            {
                SupplierName = "Supplier A",
                OfferName = "Premium Certificate Package",
                Quantity = 35,
                UnitPrice = 2m
            },

            new SupplyOffer
            {
                SupplierName = "Supplier B",
                OfferName = "Standard Certificate Package",
                Quantity = 35,
                UnitPrice = 1.5m
            },

            new SupplyOffer
            {
                SupplierName = "Supplier C",
                OfferName = "Luxury Certificate Package",
                Quantity = 35,
                UnitPrice = 2.5m
            }
        };

        // Category 2: Hospitality
        private List<SupplyOffer> hospitality = new List<SupplyOffer>
        {
            new SupplyOffer
            {
                SupplierName = "Supplier D",
                OfferName = "Coffee & Drinks Package",
                Quantity = 100,
                UnitPrice = 2m
            },

            new SupplyOffer
            {
                SupplierName = "Supplier E",
                OfferName = "Sandwiches & Snacks Package",
                Quantity = 100,
                UnitPrice = 3m
            },

            new SupplyOffer
            {
                SupplierName = "Supplier F",
                OfferName = "Full Hospitality Package",
                Quantity = 100,
                UnitPrice = 4m
            }
        };

        // Category 3: Graduation Gifts
        private List<SupplyOffer> gifts = new List<SupplyOffer>
        {
            new SupplyOffer
            {
                SupplierName = "Supplier G",
                OfferName = "Customized Mugs Package",
                Quantity = 35,
                UnitPrice = 2m
            },

            new SupplyOffer
            {
                SupplierName = "Supplier H",
                OfferName = "Graduation Bags Package",
                Quantity = 35,
                UnitPrice = 3m
            },

            new SupplyOffer
            {
                SupplierName = "Supplier I",
                OfferName = "Premium Gift Boxes Package",
                Quantity = 35,
                UnitPrice = 4m
            }
        };


        // Display Offers Page
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        // Receive Selected Offers
        [HttpPost]
        public IActionResult CreateRequest(
            int? certificateChoice,
            int? hospitalityChoice,
            int? giftChoice)
        {
            // Validate selections

            if (!certificateChoice.HasValue ||
                !hospitalityChoice.HasValue ||
                !giftChoice.HasValue ||
                certificateChoice < 0 || certificateChoice > 2 ||
                hospitalityChoice < 0 || hospitalityChoice > 2 ||
                giftChoice < 0 || giftChoice > 2)
            {
                return RedirectToAction("Index");
            }


            // Save Certificate Offer in Session

            HttpContext.Session.SetString(
                "CertificateOffer",
                JsonSerializer.Serialize(
                    certificates[certificateChoice.Value]
                )
            );


            // Save Hospitality Offer in Session

            HttpContext.Session.SetString(
                "HospitalityOffer",
                JsonSerializer.Serialize(
                    hospitality[hospitalityChoice.Value]
                )
            );


            // Save Gift Offer in Session

            HttpContext.Session.SetString(
                "GiftOffer",
                JsonSerializer.Serialize(
                    gifts[giftChoice.Value]
                )
            );


            return RedirectToAction("Review");
        }


        // Review Selected Offers
        [HttpGet]
        public IActionResult Review()
        {
            // Retrieve Offers from Session

            string? certificateJson =
                HttpContext.Session.GetString("CertificateOffer");

            string? hospitalityJson =
                HttpContext.Session.GetString("HospitalityOffer");

            string? giftJson =
                HttpContext.Session.GetString("GiftOffer");


            // Check if all offers are selected

            if (string.IsNullOrEmpty(certificateJson) ||
                string.IsNullOrEmpty(hospitalityJson) ||
                string.IsNullOrEmpty(giftJson))
            {
                return RedirectToAction("Index");
            }


            // Deserialize JSON into Objects

            SupplyOffer? certificate =
                JsonSerializer.Deserialize<SupplyOffer>(certificateJson);

            SupplyOffer? hospitalityOffer =
                JsonSerializer.Deserialize<SupplyOffer>(hospitalityJson);

            SupplyOffer? gift =
                JsonSerializer.Deserialize<SupplyOffer>(giftJson);


            if (certificate == null ||
                hospitalityOffer == null ||
                gift == null)
            {
                return RedirectToAction("Index");
            }


            // Calculate Each Offer Total

            decimal certificateTotal =
                certificate.Quantity * certificate.UnitPrice;

            decimal hospitalityTotal =
                hospitalityOffer.Quantity * hospitalityOffer.UnitPrice;

            decimal giftTotal =
                gift.Quantity * gift.UnitPrice;


            // Calculate Total Price

            decimal totalPrice =
                certificateTotal +
                hospitalityTotal +
                giftTotal;


            // Calculate Discount Percentage

            decimal discountPercentage = 0;

            if (totalPrice > 500)
            {
                discountPercentage = 0.15m;
            }
            else if (totalPrice > 300)
            {
                discountPercentage = 0.10m;
            }
            else
            {
                discountPercentage = 0;
            }


            // Calculate Discount Amount

            decimal discountAmount =
                totalPrice * discountPercentage;


            // Calculate Final Total

            decimal finalTotal =
                totalPrice - discountAmount;


            // Send Data to View

            ViewBag.Certificate = certificate;
            ViewBag.Hospitality = hospitalityOffer;
            ViewBag.Gift = gift;

            ViewBag.CertificateTotal = certificateTotal;
            ViewBag.HospitalityTotal = hospitalityTotal;
            ViewBag.GiftTotal = giftTotal;

            ViewBag.TotalPrice = totalPrice;
            ViewBag.DiscountPercentage = discountPercentage * 100;
            ViewBag.DiscountAmount = discountAmount;
            ViewBag.FinalTotal = finalTotal;


            return View();
        }


        // About Page
        [HttpGet]
        public IActionResult About()
        {
            return View();
        }
    }
}

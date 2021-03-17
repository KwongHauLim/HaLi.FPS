# HaLi.FPS
轉數快 Faster Payment System (FPS)
* Generate QR code content

```cs
  // Create FPS object
  var fps = new FPS
  {
      FpsID = "1234567", // Current is 7 digits, further will be 10 digits
      BankCode = "004",  // HSBC
      InitiationMethod = FPS.InitiationType.Dynamic, // mean the payment is one time only
      TransactionAmount = 1.00M, // $1
      TransactionAmountEditable = false, // cannot edit by payer
      ReferenceLabel = "PayRef00001" // payment reference, HSBC is must provide the field
  };
  var qrCode = fps.ToString(); // QR Code content
  // ... Generate real QR Code image by other library
```

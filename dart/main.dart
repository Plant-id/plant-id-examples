import 'identification.dart';
import 'request_handler.dart';
import 'dart:collection';
import 'dart:io';
import 'dart:convert';

const IDENTIFY_PLANT_ENDPOINT = "https://api.plant.id/v2/identify";

void main() {
  //Get file here;
  File file = new File("plant.jpg");
  HashMap<String, dynamic> data = new HashMap();
  HashMap<String, dynamic> headers = new HashMap();
  headers['Content-Type'] = 'application/json';
  headers['Api-Key'] = '-- ask for one: https://web.plant.id/api-access-request/ --';
  Future<dynamic> base64FutureImage = getImageInBase64(file);
  base64FutureImage.then((base64Image) {
    data['images'] = [base64Image];
    data['plant_details'] = ["wiki_description", "url", "wiki_image"];
    Future<Identification> futureIdentification = RequestHandler.request(IDENTIFY_PLANT_ENDPOINT, data, headers);
    futureIdentification.then((identification) {
      if(identification != null){
        print(identification.suggestions);
        print(identification.images);
        print(identification.id);
      }
    });
  });
}

getImageInBase64(File file) async {
  var imageBytes = await file.readAsBytes();
  String base64 = base64Encode(imageBytes);
  return base64;
}
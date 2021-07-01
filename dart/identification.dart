class Identification {
  final int id;
  final List images;
  final List suggestions;

  Identification({this.id, this.images, this.suggestions});

  factory Identification.fromJson(Map<String, dynamic> json) {
    return Identification(
        id: json['id'],
        images: json['images'],
        suggestions: json['suggestions']);
  }
}

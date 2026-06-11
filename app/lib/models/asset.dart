class Asset {
  final String assetId;
  final String name;
  final String type;
  final String location;
  final String? description;
  final String qrCodePayload;
  final String createdAt;

  const Asset({
    required this.assetId,
    required this.name,
    required this.type,
    required this.location,
    this.description,
    required this.qrCodePayload,
    required this.createdAt,
  });

  factory Asset.fromJson(Map<String, dynamic> j) => Asset(
        assetId: j['assetId'] as String,
        name: j['name'] as String,
        type: j['type'] as String,
        location: j['location'] as String,
        description: j['description'] as String?,
        qrCodePayload: j['qrCodePayload'] as String,
        createdAt: j['createdAt'] as String,
      );
}

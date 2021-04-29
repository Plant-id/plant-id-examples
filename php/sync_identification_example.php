<?php

function encodeImages($images) {
    $encoded_images = [];
    foreach ($images as $image) {
        $encoded_images[] = base64_encode(file_get_contents($image));
    }
    return $encoded_images;
}

function identifyPlants($file_names) {
    $api_key = 'ask for one: https://web.plant.id/api-access-request/';
    $params  = [
        'api_key'        => $api_key,
        'images'         => encodeImages($file_names),
        'modifiers'      => ['crops_fast', 'similar_images'],
        'plant_language' => 'en',
        'plant_details'  => [
            'common_names',
            'url',
            'name_authority',
            'wiki_description',
            'taxonomy',
            'synonyms'
        ],
    ];

    $ch = curl_init();
    curl_setopt_array(
        $ch,
        [
            CURLOPT_URL            => 'https://api.plant.id/v2/identify',
            CURLOPT_RETURNTRANSFER => true,
            CURLOPT_POST           => true,
            CURLOPT_POSTFIELDS     => json_encode($params),
            CURLOPT_HTTPHEADER     => ['Content-Type:application/json']
        ]
    );

    $result = curl_exec($ch);
    curl_close($ch);

    return $result;
}

print_r(identifyPlants(['photo1.jpg', 'photo2.jpg']));
